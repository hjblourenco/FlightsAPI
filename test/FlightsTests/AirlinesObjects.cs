using System;
using System.Collections.Generic;

public class AirlinesObjects
 {        
        
        public static Airline CreateTestAirlineObject()
        {
            Airline airline = new Airline()
            {
                AirlineId = Guid.NewGuid(),
                Name = GenerateRandomStrings.RandomString(30),

            };
            return airline;
        }

        public static IEnumerable<Airline> CreateTestAirlineObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return CreateTestAirlineObject();
            }
        }
 }            