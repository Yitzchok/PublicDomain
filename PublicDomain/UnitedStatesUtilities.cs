using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
#if !(NOSTATES)
    /// <summary>
    /// Methods and date related to the United States, such as a list
    /// of States.
    /// </summary>
    public static class UnitedStatesUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly USState[] States;

        static UnitedStatesUtilities()
        {
            int stateId = 1;
            States = new USState[] {
				new USState(stateId++, "Alabama", "AL"),
				new USState(stateId++, "Alaska", "AK"),
				new USState(stateId++, "Arizona", "AZ"),
				new USState(stateId++, "Arkansas", "AR"),
				new USState(stateId++, "California", "CA"),
				new USState(stateId++, "Colorado", "CO"),
				new USState(stateId++, "Connecticut", "CT"),
				new USState(stateId++, "Delaware", "DE"),
				new USState(stateId++, "District of Columbia", "DC"),
				new USState(stateId++, "Florida", "FL"),
				new USState(stateId++, "Georgia", "GA"),
				new USState(stateId++, "Hawaii", "HI"),
				new USState(stateId++, "Idaho", "ID"),
				new USState(stateId++, "Illinois", "IL"),
				new USState(stateId++, "Indiana", "IN"),
				new USState(stateId++, "Iowa", "IA"),
				new USState(stateId++, "Kansas", "KS"),
				new USState(stateId++, "Kentucky", "KY"),
				new USState(stateId++, "Louisiana", "LA"),
				new USState(stateId++, "Maine", "ME"),
				new USState(stateId++, "Maryland", "MD"),
				new USState(stateId++, "Massachusetts", "MA"),
				new USState(stateId++, "Michigan", "MI"),
				new USState(stateId++, "Minnesota", "MN"),
				new USState(stateId++, "Mississippi", "MS"),
				new USState(stateId++, "Missouri", "MO"),
				new USState(stateId++, "Montana", "MT"),
				new USState(stateId++, "Nebraska", "NE"),
				new USState(stateId++, "Nevada", "NV"),
				new USState(stateId++, "New Hampshire", "NH"),
				new USState(stateId++, "New Jersey", "NJ"),
				new USState(stateId++, "New Mexico", "NM"),
				new USState(stateId++, "New York", "NY"),
				new USState(stateId++, "North Carolina", "NC"),
				new USState(stateId++, "North Dakota", "ND"),
				new USState(stateId++, "Ohio", "OH"),
				new USState(stateId++, "Oklahoma", "OK"),
				new USState(stateId++, "Oregon", "OR"),
				new USState(stateId++, "Pennsylvania", "PA"),
				new USState(stateId++, "Rhode Island", "RI"),
				new USState(stateId++, "South Carolina", "SC"),
				new USState(stateId++, "South Dakota", "SD"),
				new USState(stateId++, "Tennessee", "TN"),
				new USState(stateId++, "Texas", "TX"),
				new USState(stateId++, "Utah", "UT"),
				new USState(stateId++, "Vermont", "VT"),
				new USState(stateId++, "Viginia", "VA"),
				new USState(stateId++, "Washington", "WA"),
				new USState(stateId++, "West Virginia", "WV"),
				new USState(stateId++, "Wisconsin", "WI"),
				new USState(stateId++, "Wyoming", "WY")
			};
        }

        /// <summary>
        /// Attempts to find a <see cref="PublicDomain.UnitedStatesUtilities.USState"/>
        /// by its abbreviate.
        /// </summary>
        /// <param name="abbreviation">The abbreviation of the state to search for. Not case sensitive.</param>
        /// <returns>
        /// The <see cref="PublicDomain.UnitedStatesUtilities.USState"/> that represents the
        /// <c>abbreviation</c>, or if it is not found, throws a <see cref="PublicDomain.UnitedStatesUtilities.StateNotFoundException"/>
        /// </returns>
        /// <exception cref="PublicDomain.UnitedStatesUtilities.StateNotFoundException"></exception>
        public static USState GetStateByAbbrivation(string abbreviation)
        {
            if (abbreviation == null)
            {
                throw new ArgumentNullException("abbreviation");
            }
            abbreviation = abbreviation.ToLower().Trim();
            foreach (USState state in States)
            {
                if (state.Abbreviation.ToLower() == abbreviation)
                {
                    return state;
                }
            }
            throw new StateNotFoundException(abbreviation);
        }

        /// <summary>
        /// Attempts to find a <see cref="PublicDomain.UnitedStatesUtilities.USState"/>
        /// by its name.
        /// </summary>
        /// <param name="stateName">The name of the state to search for. Not case sensitive.</param>
        /// <returns>
        /// The <see cref="PublicDomain.UnitedStatesUtilities.USState"/> that represents the
        /// <c>abbreviation</c>, or if it is not found, throws a <see cref="PublicDomain.UnitedStatesUtilities.StateNotFoundException"/>
        /// </returns>
        /// <exception cref="PublicDomain.UnitedStatesUtilities.StateNotFoundException"></exception>
        public static USState GetStateByName(string stateName)
        {
            if (stateName == null)
            {
                throw new ArgumentNullException("stateName");
            }
            stateName = stateName.ToLower().Trim();
            foreach (USState state in States)
            {
                if (state.Name.ToLower().Equals(stateName))
                {
                    return state;
                }
            }
            throw new StateNotFoundException(stateName);
        }

        /// <summary>
        /// Represents information about a state from the United States
        /// of America.
        /// </summary>
        [Serializable]
        public struct USState
        {
            /// <summary>
            /// 
            /// </summary>
            public int UniqueId;

            /// <summary>
            /// 
            /// </summary>
            public string Name;

            /// <summary>
            /// 
            /// </summary>
            public string Abbreviation;

            /// <summary>
            /// Initializes a new instance of the <see cref="USState"/> class.
            /// </summary>
            /// <param name="uniqueId">The unique id.</param>
            /// <param name="name">The name.</param>
            /// <param name="abbreviation">The abbreviation.</param>
            public USState(int uniqueId, string name, string abbreviation)
            {
                this.UniqueId = uniqueId;
                this.Name = name;
                this.Abbreviation = abbreviation;
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <param name="toUpperCase">if set to <c>true</c> [to upper case].</param>
            /// <returns></returns>
            public string GetName(bool toUpperCase)
            {
                return toUpperCase ? Name.ToUpper() : Name.ToLower();
            }

            /// <summary>
            /// Gets the abbreviation.
            /// </summary>
            /// <param name="toUpperCase">if set to <c>true</c> [to upper case].</param>
            /// <returns></returns>
            public string GetAbbreviation(bool toUpperCase)
            {
                return toUpperCase ? Abbreviation.ToUpper() : Abbreviation.ToLower();
            }
        }

        /// <summary>
        /// Thrown when the state being searched for does not exist.
        /// </summary>
        [Serializable]
        public class StateNotFoundException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="StateNotFoundException"/> class.
            /// </summary>
            public StateNotFoundException() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="StateNotFoundException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            public StateNotFoundException(string message) : base(message) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="StateNotFoundException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="inner">The inner.</param>
            public StateNotFoundException(string message, Exception inner) : base(message, inner) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="StateNotFoundException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            protected StateNotFoundException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }
#endif
}
