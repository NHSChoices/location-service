namespace GoatTrip.RestApi.Models {
    public class CoordinateModel {
        public CoordinateModel(float x, float y) {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }
}