namespace Talabat2.core.Entities.Identity
{
    public class Address
    {
        public int Id { get; set; }
        public string  FirstName { get; set; }
        public string  LastName { get; set; }
        public string  Street { get; set; }
        public string  City { get; set; }
        public string  Country { get; set; }


        public string  AppUserId { get; set; }  //FK    //the relation right new is one to one
                                                //we make proprtes for FirstName & LastName for user to any change request in the future
        public AppUser User { get; set; }  // N P One
    }
}