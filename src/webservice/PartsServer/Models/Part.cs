namespace PartsService.Models
{
    public class Car
    {
        public string CarID { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public double Cena { get; set; }
        public int Rocznik { get; set; }
        public string Href => $"api/cars/{CarID}";
    }
}
