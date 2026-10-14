using MunicipalityIssueReporter.Models;

namespace MunicipalityIssueReporter.Services
{
    /// <summary>
    /// In-memory store for local events and announcements.
    ///
    /// Data structures used and why:
    ///  - Dictionary&lt;int, LocalEvent&gt;                 (hash table)  O(1) lookup of an event by Id.
    ///  - SortedDictionary&lt;DateTime, List&lt;LocalEvent&gt;&gt;          keeps events grouped by day in chronological order.
    ///  - Dictionary&lt;string, List&lt;LocalEvent&gt;&gt;       (hash table)  category index so a category search skips non-matching events.
    ///  - HashSet&lt;string&gt;                            (set)         unique list of categories for the filter dropdown.
    ///  - HashSet&lt;DateTime&gt;                          (set)         unique event dates with no duplicates.
    ///  - PriorityQueue&lt;Announcement, ...&gt;                         urgent announcements always come out first.
    ///  - Stack&lt;LocalEvent&gt;                                       recently viewed events (last viewed on top).
    ///  - Queue&lt;SearchRecord&gt;                                     rolling history of the last few searches (oldest drops off).
    ///  - Dictionary&lt;string, int&gt;                    (hash table)  how interested the user is in each category.
    /// </summary>
    public static class EventService
    {
        private const int MaxSearchHistory = 10;
        private const int MaxRecentlyViewed = 5;

        // Interest weights used by the recommendation engine.
        private const int CategorySearchWeight = 2;
        private const int EventViewWeight = 3;

        private static readonly Dictionary<int, LocalEvent> eventsById = new();

        private static readonly SortedDictionary<DateTime, List<LocalEvent>> eventsByDate = new();

        private static readonly Dictionary<string, List<LocalEvent>> eventsByCategory =
            new(StringComparer.OrdinalIgnoreCase);

        private static readonly HashSet<string> categories = new(StringComparer.OrdinalIgnoreCase);

        private static readonly HashSet<DateTime> eventDates = new();

        // Priority is (priority level, -ticks) so that within the same level the newest comes first.
        private static readonly PriorityQueue<Announcement, (int Level, long Newest)> announcements = new();

        private static readonly Stack<LocalEvent> recentlyViewed = new();

        private static readonly Queue<SearchRecord> searchHistory = new();

        private static readonly Dictionary<string, int> categoryInterest = new(StringComparer.OrdinalIgnoreCase);

        private static int nextEventId = 1;

        static EventService()
        {
            SeedEvents();
            SeedAnnouncements();
        }

        #region Events

        public static void AddEvent(LocalEvent localEvent)
        {
            localEvent.Id = nextEventId++;

            eventsById[localEvent.Id] = localEvent;

            DateTime day = localEvent.Date.Date;
            if (!eventsByDate.TryGetValue(day, out List<LocalEvent>? sameDay))
            {
                sameDay = new List<LocalEvent>();
                eventsByDate.Add(day, sameDay);
            }
            sameDay.Add(localEvent);

            if (!eventsByCategory.TryGetValue(localEvent.Category, out List<LocalEvent>? sameCategory))
            {
                sameCategory = new List<LocalEvent>();
                eventsByCategory.Add(localEvent.Category, sameCategory);
            }
            sameCategory.Add(localEvent);

            // HashSet ignores duplicates, so each category and date only appears once.
            categories.Add(localEvent.Category);
            eventDates.Add(day);
        }

        public static LocalEvent? GetEventById(int id)
        {
            return eventsById.TryGetValue(id, out LocalEvent? found) ? found : null;
        }

        /// <summary>
        /// All events in date order (comes straight out of the SortedDictionary).
        /// </summary>
        public static List<LocalEvent> GetAllEvents()
        {
            return eventsByDate.Values
                .SelectMany(day => day.OrderBy(e => e.Date))
                .ToList();
        }

        /// <summary>
        /// Upcoming events only (today onwards), in date order.
        /// </summary>
        public static List<LocalEvent> GetUpcomingEvents()
        {
            DateTime today = DateTime.Today;

            return eventsByDate
                .Where(pair => pair.Key >= today)
                .SelectMany(pair => pair.Value.OrderBy(e => e.Date))
                .ToList();
        }

        public static List<string> GetCategories()
        {
            return categories.OrderBy(c => c).ToList();
        }

        public static List<DateTime> GetUniqueEventDates()
        {
            return eventDates.OrderBy(d => d).ToList();
        }

        public static int GetEventCount()
        {
            return eventsById.Count;
        }

        #endregion

        #region Search

        /// <summary>
        /// Searches events by keyword, category and/or date range.
        /// Any blank / null criteria are ignored. Non-empty searches are recorded
        /// so the recommendation engine can learn from them.
        /// </summary>
        public static List<LocalEvent> Search(string? keyword, string? category, DateTime? fromDate, DateTime? toDate)
        {
            keyword = keyword?.Trim() ?? string.Empty;
            category = category?.Trim() ?? string.Empty;

            IEnumerable<LocalEvent> results;

            if (category.Length > 0)
            {
                // Category given: jump straight to that category's list via the hash table.
                results = eventsByCategory.TryGetValue(category, out List<LocalEvent>? inCategory)
                    ? inCategory
                    : Enumerable.Empty<LocalEvent>();
            }
            else
            {
                // No category: walk the sorted dictionary, skipping whole days outside the range.
                results = eventsByDate
                    .Where(pair => (!fromDate.HasValue || pair.Key >= fromDate.Value.Date) &&
                                   (!toDate.HasValue || pair.Key <= toDate.Value.Date))
                    .SelectMany(pair => pair.Value);
            }

            if (fromDate.HasValue)
                results = results.Where(e => e.Date.Date >= fromDate.Value.Date);

            if (toDate.HasValue)
                results = results.Where(e => e.Date.Date <= toDate.Value.Date);

            if (keyword.Length > 0)
                results = results.Where(e => MatchesKeyword(e, keyword));

            List<LocalEvent> list = results
                .OrderBy(e => e.Date)
                .ThenBy(e => e.Title)
                .ToList();

            if (keyword.Length > 0 || category.Length > 0 || fromDate.HasValue || toDate.HasValue)
            {
                RecordSearch(new SearchRecord
                {
                    Keyword = keyword,
                    Category = category,
                    FromDate = fromDate?.Date,
                    ToDate = toDate?.Date,
                    SearchedAt = DateTime.Now
                });
            }

            return list;
        }

        private static bool MatchesKeyword(LocalEvent e, string keyword)
        {
            return e.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                   e.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                   e.Venue.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                   e.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase);
        }

        private static void RecordSearch(SearchRecord record)
        {
            searchHistory.Enqueue(record);

            // Keep the queue a fixed size: the oldest search falls off the front.
            while (searchHistory.Count > MaxSearchHistory)
                searchHistory.Dequeue();

            if (record.Category.Length > 0)
                AddInterest(record.Category, CategorySearchWeight);
        }

        public static List<SearchRecord> GetSearchHistory()
        {
            // Newest first for display.
            return searchHistory.Reverse().ToList();
        }

        #endregion

        #region Recently viewed

        /// <summary>
        /// Pushes an event onto the recently viewed stack. If it was already there
        /// it is moved to the top, and the stack is capped at MaxRecentlyViewed.
        /// </summary>
        public static void RecordView(LocalEvent localEvent)
        {
            // Stack enumerates top -> bottom, so this keeps the most recent ones.
            List<LocalEvent> keep = recentlyViewed
                .Where(e => e.Id != localEvent.Id)
                .Take(MaxRecentlyViewed - 1)
                .ToList();

            recentlyViewed.Clear();

            // Push oldest first so the order is preserved.
            for (int i = keep.Count - 1; i >= 0; i--)
                recentlyViewed.Push(keep[i]);

            recentlyViewed.Push(localEvent);

            AddInterest(localEvent.Category, EventViewWeight);
        }

        public static List<LocalEvent> GetRecentlyViewed()
        {
            return recentlyViewed.ToList();
        }

        #endregion

        #region Recommendations

        private static void AddInterest(string category, int amount)
        {
            categoryInterest.TryGetValue(category, out int current);
            categoryInterest[category] = current + amount;
        }

        /// <summary>
        /// Scores every upcoming event against what the user has searched for and viewed:
        ///  - interest in the event's category (from category searches and event views)
        ///  - keyword matches against recent searches
        ///  - falling inside a date range the user searched for
        ///  - a small boost for events happening within the next week
        /// Events the user has already viewed are left out. If there is no history yet,
        /// the soonest upcoming events are returned instead.
        /// </summary>
        public static List<Recommendation> GetRecommendations(int count = 5)
        {
            List<LocalEvent> upcoming = GetUpcomingEvents();
            HashSet<int> viewedIds = recentlyViewed.Select(e => e.Id).ToHashSet();

            bool hasHistory = searchHistory.Count > 0 || categoryInterest.Count > 0;

            if (!hasHistory)
            {
                return upcoming
                    .Take(count)
                    .Select(e => new Recommendation { Event = e, Score = 0, Reason = "Coming up soon" })
                    .ToList();
            }

            List<string> recentKeywords = searchHistory
                .Where(s => s.Keyword.Length > 0)
                .Select(s => s.Keyword)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            List<SearchRecord> dateSearches = searchHistory
                .Where(s => s.FromDate.HasValue || s.ToDate.HasValue)
                .ToList();

            DateTime nextWeek = DateTime.Today.AddDays(7);

            List<Recommendation> scored = new();

            foreach (LocalEvent e in upcoming)
            {
                if (viewedIds.Contains(e.Id))
                    continue;

                int categoryScore = categoryInterest.TryGetValue(e.Category, out int interest) ? interest : 0;

                string? matchedKeyword = recentKeywords.FirstOrDefault(k => MatchesKeyword(e, k));
                int keywordScore = recentKeywords.Count(k => MatchesKeyword(e, k)) * 2;

                int dateScore = dateSearches.Count(s =>
                    (!s.FromDate.HasValue || e.Date.Date >= s.FromDate.Value) &&
                    (!s.ToDate.HasValue || e.Date.Date <= s.ToDate.Value));

                int score = categoryScore + keywordScore + dateScore;

                if (score == 0)
                    continue;

                if (e.Date <= nextWeek)
                    score += 1;

                string reason;
                if (categoryScore >= keywordScore && categoryScore >= dateScore)
                    reason = $"You showed interest in {e.Category}";
                else if (keywordScore >= dateScore && matchedKeyword != null)
                    reason = $"Matches your search \"{matchedKeyword}\"";
                else
                    reason = "In a date range you searched";

                scored.Add(new Recommendation { Event = e, Score = score, Reason = reason });
            }

            List<Recommendation> top = scored
                .OrderByDescending(r => r.Score)
                .ThenBy(r => r.Event.Date)
                .Take(count)
                .ToList();

            // Top up with soonest events so the list is never empty.
            if (top.Count < count)
            {
                HashSet<int> used = top.Select(r => r.Event.Id).ToHashSet();

                top.AddRange(upcoming
                    .Where(e => !used.Contains(e.Id) && !viewedIds.Contains(e.Id))
                    .Take(count - top.Count)
                    .Select(e => new Recommendation { Event = e, Score = 0, Reason = "Coming up soon" }));
            }

            return top;
        }

        #endregion

        #region Announcements

        public static void AddAnnouncement(Announcement announcement)
        {
            announcements.Enqueue(announcement, ((int)announcement.Priority, -announcement.DatePosted.Ticks));
        }

        /// <summary>
        /// Returns announcements most-urgent first. Works on a copy so the
        /// original priority queue is not emptied.
        /// </summary>
        public static List<Announcement> GetAnnouncementsByPriority()
        {
            PriorityQueue<Announcement, (int Level, long Newest)> copy = new(announcements.UnorderedItems);

            List<Announcement> ordered = new();
            while (copy.TryDequeue(out Announcement? next, out _))
                ordered.Add(next);

            return ordered;
        }

        #endregion

        #region Sample data

        private static void SeedEvents()
        {
            DateTime today = DateTime.Today;

            AddEvent(new LocalEvent { Title = "Community Clean-Up Day", Category = "Environment", Date = today.AddDays(2).AddHours(8), Venue = "Riverside Park", Description = "Join neighbours to clean up the park and riverbank. Gloves and bags provided." });
            AddEvent(new LocalEvent { Title = "Ward 12 Community Meeting", Category = "Public Meeting", Date = today.AddDays(3).AddHours(18), Venue = "Ward 12 Community Hall", Description = "Ward councillor feedback session on roads, water and electricity projects." });
            AddEvent(new LocalEvent { Title = "Free Health Screening", Category = "Health", Date = today.AddDays(4).AddHours(9), Venue = "Central Clinic", Description = "Free blood pressure, diabetes and HIV screening. No appointment needed." });
            AddEvent(new LocalEvent { Title = "Youth Soccer Tournament", Category = "Sports", Date = today.AddDays(5).AddHours(10), Venue = "Municipal Sports Grounds", Description = "Under-15 and under-18 teams from all wards compete for the Mayor's Cup." });
            AddEvent(new LocalEvent { Title = "Farmers & Crafters Market", Category = "Markets", Date = today.AddDays(6).AddHours(8), Venue = "Town Square", Description = "Local fresh produce, crafts and food stalls supporting small businesses." });
            AddEvent(new LocalEvent { Title = "Water Saving Workshop", Category = "Environment", Date = today.AddDays(8).AddHours(14), Venue = "Library Auditorium", Description = "Practical tips to reduce household water use and spot leaks early." });
            AddEvent(new LocalEvent { Title = "Heritage Day Celebration", Category = "Arts & Culture", Date = today.AddDays(9).AddHours(11), Venue = "Civic Centre Lawns", Description = "Music, dance and traditional food celebrating our community's heritage." });
            AddEvent(new LocalEvent { Title = "Budget Public Participation", Category = "Public Meeting", Date = today.AddDays(10).AddHours(17), Venue = "Council Chambers", Description = "Have your say on the draft municipal budget and service priorities." });
            AddEvent(new LocalEvent { Title = "Small Business Skills Day", Category = "Education", Date = today.AddDays(11).AddHours(9), Venue = "Enterprise Hub", Description = "Workshops on registering a business, bookkeeping and accessing funding." });
            AddEvent(new LocalEvent { Title = "Fun Run & Walk (5km)", Category = "Sports", Date = today.AddDays(12).AddHours(7), Venue = "Starts at Town Hall", Description = "Family fun run and walk. All ages welcome, registration on the day." });
            AddEvent(new LocalEvent { Title = "Blood Donation Drive", Category = "Health", Date = today.AddDays(13).AddHours(10), Venue = "Community Centre", Description = "Help save lives. Bring your ID and eat a meal before donating." });
            AddEvent(new LocalEvent { Title = "Tree Planting Initiative", Category = "Environment", Date = today.AddDays(15).AddHours(9), Venue = "Greenfield Primary School", Description = "Planting 200 indigenous trees along school and street verges." });
            AddEvent(new LocalEvent { Title = "Local Artists Exhibition", Category = "Arts & Culture", Date = today.AddDays(16).AddHours(10), Venue = "Municipal Art Gallery", Description = "Paintings, sculpture and photography by artists from our area." });
            AddEvent(new LocalEvent { Title = "Matric Study Camp", Category = "Education", Date = today.AddDays(18).AddHours(8), Venue = "Public Library", Description = "Free revision sessions for Grade 12 learners ahead of final exams." });
            AddEvent(new LocalEvent { Title = "Netball League Finals", Category = "Sports", Date = today.AddDays(20).AddHours(13), Venue = "Indoor Sports Centre", Description = "Season finals of the community netball league. Free entry." });
            AddEvent(new LocalEvent { Title = "Night Market", Category = "Markets", Date = today.AddDays(21).AddHours(17), Venue = "Town Square", Description = "Evening market with food trucks, live music and local vendors." });
            AddEvent(new LocalEvent { Title = "Recycling Awareness Day", Category = "Environment", Date = today.AddDays(23).AddHours(10), Venue = "Waste Drop-off Centre", Description = "Learn what can be recycled and how separation at source works." });
            AddEvent(new LocalEvent { Title = "Community Safety Forum", Category = "Public Meeting", Date = today.AddDays(25).AddHours(18), Venue = "Police Station Hall", Description = "Residents, SAPS and neighbourhood watch discuss crime prevention." });
            AddEvent(new LocalEvent { Title = "Senior Citizens Wellness Day", Category = "Health", Date = today.AddDays(27).AddHours(9), Venue = "Old Age Home Grounds", Description = "Eye tests, light exercise and health talks for seniors." });
            AddEvent(new LocalEvent { Title = "Choir Festival", Category = "Arts & Culture", Date = today.AddDays(30).AddHours(15), Venue = "City Hall", Description = "School and church choirs perform in the annual choir festival." });
        }

        private static void SeedAnnouncements()
        {
            DateTime now = DateTime.Now;

            AddAnnouncement(new Announcement { Title = "Planned Water Outage", Message = "Water supply will be interrupted in Wards 4 and 5 from 08:00 to 16:00 tomorrow for pipe repairs.", DatePosted = now.AddHours(-2), Priority = AnnouncementPriority.Urgent });
            AddAnnouncement(new Announcement { Title = "Load Shedding Schedule Update", Message = "Stage 2 load shedding applies this week. Check the updated schedule for your area.", DatePosted = now.AddDays(-1), Priority = AnnouncementPriority.High });
            AddAnnouncement(new Announcement { Title = "Refuse Collection Delay", Message = "Refuse collection in Ward 8 will be one day late this week due to a public holiday.", DatePosted = now.AddDays(-2), Priority = AnnouncementPriority.Normal });
            AddAnnouncement(new Announcement { Title = "Road Closure: Main Street", Message = "Main Street will be closed between 1st and 3rd Avenue for resurfacing. Use alternative routes.", DatePosted = now.AddHours(-5), Priority = AnnouncementPriority.High });
            AddAnnouncement(new Announcement { Title = "Library Extended Hours", Message = "The public library will stay open until 20:00 on weekdays during exam season.", DatePosted = now.AddDays(-3), Priority = AnnouncementPriority.Normal });
            AddAnnouncement(new Announcement { Title = "Burst Pipe Warning", Message = "A burst pipe on Oak Road is being repaired. Expect low water pressure in surrounding streets.", DatePosted = now.AddMinutes(-30), Priority = AnnouncementPriority.Urgent });
        }

        #endregion
    }
}
