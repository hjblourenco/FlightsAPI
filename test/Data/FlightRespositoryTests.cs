using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace FlightRespositoryTests
{
    public class FlightRespositoryTests
    {
            private readonly MongoDbContext _context;
            private readonly FlightsMongoDbRepository _flightRepository;

        public FlightRespositoryTests()
        {
            IConfigurationRoot configuration = RepositoryTests.RepositoryTestsConstructor();
            _context = new MongoDbContext(configuration);
            _flightRepository = new FlightsMongoDbRepository(_context);

        }




        //Add flights
        public void CreateFlight()
        {
            var flight = FlightObject.CreateTestFlightObject();
            _flightRepository.AddFlight(flight);        
        }

        [Fact]
        public void DeleteFlight()
        {
            Task t = new Task(() =>
            {    
            var allFlights = _flightRepository.GetAllFlights().ToList();

            //If the database dont have flights then create one
            if (allFlights.Count == 0)
            {
                CreateFlight();
                
                allFlights = _flightRepository.GetAllFlights().ToList();
            }
            _flightRepository.DeleteFlight(((Flight)allFlights[0]).FlightId);
            var allFlightAfterDelete = _flightRepository.GetAllFlights().Count();
            Assert.Equal(allFlights.Count - 1, allFlightAfterDelete);
            });
            t.RunSynchronously();
        }

        [Fact]
        public void AddFlight()
        {
            //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
            //            Task t = new Task(() =>
            //{
            //CODE
            //});
            //t.RunSynchronously();
            Task t = new Task(() =>
            {            
                var allFlightsBefore = _flightRepository.GetAllFlights().ToList().Count;

                Flight testObjectToAdd = FlightObject.CreateTestFlightObject();
                _flightRepository.AddFlight(testObjectToAdd);

                var allFlightsAfter = _flightRepository.GetAllFlights().ToList().Count;
                Assert.Equal(allFlightsBefore+1,allFlightsAfter);
            });
            t.RunSynchronously();                
        }

        [Fact]
        public void ChangeFlight()
        {
            var allFlights = _flightRepository.GetAllFlights().ToList();

            //If the database dont have flights then create one
            if (allFlights.Count == 0)
            {
                CreateFlight();
                allFlights = _flightRepository.GetAllFlights().ToList();
            }

            var flight = (Flight)allFlights[0];
            flight.FlightNumber = FlightObject.RandomString(20);
            _flightRepository.UpdateFlight(flight.FlightId,flight);

            var updatedFlight = _flightRepository.GetFlightById(flight.FlightId);
            Assert.Equal(flight.FlightNumber, updatedFlight?.FlightNumber);

        }



    }
}