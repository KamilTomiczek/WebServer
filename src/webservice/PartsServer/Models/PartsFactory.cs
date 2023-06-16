namespace PartsService.Models
{
    public static class PartsFactory
    {
        static object locker = new object();
        public static Dictionary<string, Tuple<DateTime, List<Car>>> Cars = new Dictionary<string, Tuple<DateTime, List<Car>>>();

        public static void Initialize(string authorizationToken)
        {
            lock (locker)
            {
                Cars.Add(authorizationToken,
                    Tuple.Create(DateTime.UtcNow.AddHours(1), DefaultParts.ToList()));
            }
        }

        private static IEnumerable<Car> DefaultParts
        {
            get
            {
                yield return new Car
                {
                    CarID = "0545685192",
                    Marka = "Opel",
                    Model = "Astra",
                    Cena = 5600,
                    Rocznik = 2012,
                }; 
                yield return new Car
                {
                    CarID = "0541285192",
                    Marka = "Audi",
                    Model = "A5",
                    Cena = 12000,
                    Rocznik = 2018,
                };
            }
        }

        public static void ClearStaleData()
        {
            lock (locker)
            {
                var keys = Cars.Keys.ToList();
                foreach (var oneKey in keys)
                {
                    if (Cars.TryGetValue(oneKey, out Tuple<DateTime, List<Car>> result) &&
                        result.Item1 < DateTime.UtcNow)
                    {
                        Cars.Remove(oneKey);
                    }
                }
            }
        }

        static readonly Random Rng = new Random();
        public static string CreateCarID()
        {
            char[] ch = new char[10];
            for (int i = 0; i < 10; i++)
            {
                ch[i] = (char)('0' + Rng.Next(0, 9));
            }
            return new string(ch);
        }
    }
}
