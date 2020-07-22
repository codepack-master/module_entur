 public class BikeObject {
     public float stamp {get; set;}
     public int ttl {get; set;}

     public BikeStations data {get; set;}
 }

 public class BikeTest {
      public BikeStations data {get; set;}
 }

 public class BikeStations {
        
        public BikeData[] stations {get; set; }
    }


    public class BikeData  {
            
            public int station_id {get; set; }
            public string name {get; set; }
            public double lat {get; set;}
            public double lon {get; set;}
            public int capacity {get; set;}

    }
