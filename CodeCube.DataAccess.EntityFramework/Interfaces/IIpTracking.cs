namespace CodeCube.DataAccess.EntityFramework.Interfaces
{
    public interface IIpTracking
    {
        /// <summary>
        /// The client IP-address
        /// </summary>
        string IpAddress { get; set; }
    }
}
