﻿using System;
using System.Runtime.Serialization;

namespace GoatTrip.RestApi.Services {
    [Serializable]
    public class InvalidLocationQueryException
        : Exception {

        public InvalidLocationQueryException() {
        }

        public InvalidLocationQueryException(string message)
            : base(message) {
        }

        public InvalidLocationQueryException(string message, Exception inner)
            : base(message, inner) {
        }

        protected InvalidLocationQueryException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {
        }
    }
}