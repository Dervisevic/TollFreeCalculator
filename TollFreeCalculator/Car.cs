namespace TollFeeCalculator
{
    public class Car : IVehicle
    {
        public bool IsMilitary { get; set; } = false;
        public bool IsDiplomat { get; set; } = false;
        public bool IsEmergency { get; set; } = false;

        public bool GetExemptionStatus()
        {
            return IsDiplomat || IsMilitary || IsEmergency;
        }
    }
}