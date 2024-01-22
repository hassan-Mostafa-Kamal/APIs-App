﻿namespace Talabat.Apis2.ErrorsResponseHandling
{
    public class ApiValidationErrorResponse : ApiErrorResponse
    {

        public IEnumerable<string> Errors { get; set; }


        public ApiValidationErrorResponse():base(400)
        {
                Errors = new List<string>();
        }
    }
}
