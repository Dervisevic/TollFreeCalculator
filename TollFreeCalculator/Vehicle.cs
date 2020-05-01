namespace TollFeeCalculator
{
    public interface IVehicle
    {

     /**
     * @return - Wether the vehicle is exempt from tolls.
     */
        bool GetExemptionStatus();
    }
}