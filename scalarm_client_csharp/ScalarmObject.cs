using System;
namespace Scalarm
{
    public class ScalarmObject
    {
        public Scalarm.Client Client {get; set;}

        public ScalarmObject()
        {}

        public ScalarmObject(Client client)
        {
            Client = client;
        }
    }
}

