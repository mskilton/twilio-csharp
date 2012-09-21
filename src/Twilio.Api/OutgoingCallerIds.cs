﻿using RestSharp;
using RestSharp.Extensions;
using RestSharp.Validation;

namespace Twilio
{
	public partial class TwilioRestClient
	{
		/// <summary>
		/// Retrieve the details for an existing validated Outgoing Caller ID entry.
		/// Makes a GET request to an OutgoingCallerId Instance resource.
		/// </summary>
		/// <param name="outgoingCallerIdSid">The Sid of the entry to retrieve</param>
		public OutgoingCallerId GetOutgoingCallerId(string outgoingCallerIdSid)
		{
			var request = new RestRequest();
			request.Resource = "Accounts/{AccountSid}/OutgoingCallerIds/{OutgoingCallerIdSid}.json";
						request.AddParameter("OutgoingCallerIdSid", outgoingCallerIdSid, ParameterType.UrlSegment);

			return Execute<OutgoingCallerId>(request);
		}

		/// <summary>
		/// Returns a list of validated outgoing caller IDs. The list includes paging information.
		/// Makes a GET request to an OutgoingCallerIds List resource.
		/// </summary>
		public OutgoingCallerIdResult ListOutgoingCallerIds()
		{
			return ListOutgoingCallerIds(null, null, null, null);
		}

		/// <summary>
		/// Returns a filtered list of validated outgoing caller IDs. The list includes paging information.
		/// Makes a GET request to an OutgoingCallerIds List resource.
		/// </summary>
		/// <param name="phoneNumber">If present, filter the list by the value provided</param>
		/// <param name="friendlyName">If present, filter the list by the value provided</param>
		/// <param name="pageNumber">If present, start the results from the specified page</param>
		/// <param name="count">If present, return the specified number of results, up to 1000</param>
		/// <returns></returns>
		public OutgoingCallerIdResult ListOutgoingCallerIds(string phoneNumber, string friendlyName, int? pageNumber, int? count)
		{
			var request = new RestRequest();
			request.Resource = "Accounts/{AccountSid}/OutgoingCallerIds.json";
			
			if (phoneNumber.HasValue()) request.AddParameter("PhoneNumber", phoneNumber);
			if (friendlyName.HasValue()) request.AddParameter("FriendlyName", friendlyName);
			if (pageNumber.HasValue) request.AddParameter("Page", pageNumber.Value);
			if (count.HasValue) request.AddParameter("PageSize", count.Value);

			return Execute<OutgoingCallerIdResult>(request);
		}

		/// <summary>
		/// Adds a new validated CallerID to your account. 
		/// After making this request, Twilio will return to you a validation code and dial the phone number given to perform validation. 
		/// The code returned must be entered via the phone before the CallerID will be added to your account.
		/// Makes a POST request to an OutgoingCallerIds List resource.
		/// </summary>
		/// <param name="phoneNumber">The phone number to verify. Should be formatted with a '+' and country code e.g., +16175551212 (E.164 format). Twilio will also accept unformatted US numbers e.g., (415) 555-1212, 415-555-1212.</param>
		/// <param name="friendlyName">A human readable description for the new caller ID with maximum length 64 characters. Defaults to a nicely formatted version of the number.</param>
		/// <param name="callDelay">The number of seconds, between 0 and 60, to delay before initiating the validation call. Defaults to 0.</param>
		/// <param name="extension">Digits to dial after connecting the validation call.</param>
        /// 
		public ValidationRequestResult AddOutgoingCallerId(string phoneNumber, string friendlyName, int? callDelay, string extension, string CallBackUrl)
		{
			Require.Argument("PhoneNumber", phoneNumber);
			if (callDelay.HasValue) Validate.IsBetween(callDelay.Value, 0, 60);

			var request = new RestRequest(Method.POST);
			request.Resource = "Accounts/{AccountSid}/OutgoingCallerIds.json";
						request.AddParameter("PhoneNumber", phoneNumber);

			if (friendlyName.HasValue()) request.AddParameter("FriendlyName", friendlyName);
			if (callDelay.HasValue) request.AddParameter("CallDelay", callDelay.Value);
			if (extension.HasValue()) request.AddParameter("Extension", extension);
            if (CallBackUrl.HasValue()) request.AddParameter("StatusCallback", CallBackUrl);

			return Execute<ValidationRequestResult>(request);
		}

		/// <summary>
		/// Update the FriendlyName associated with a validated outgoing caller ID entry.
		/// Makes a POST request to an OutgoingCallerId Instance resource.
		/// </summary>
		/// <param name="outgoingCallerIdSid">The Sid of the outgoing caller ID entry</param>
		/// <param name="friendlyName">The name to update the FriendlyName to</param>
		public OutgoingCallerId UpdateOutgoingCallerIdName(string outgoingCallerIdSid, string friendlyName)
		{
			Require.Argument("OutgoingCallerIdSid", outgoingCallerIdSid);
			Require.Argument("FriendlyName", friendlyName);
			Validate.IsValidLength(friendlyName, 64);

			var request = new RestRequest(Method.POST);
			request.Resource = "Accounts/{AccountSid}/OutgoingCallerIds/{OutgoingCallerIdSid}.json";
			
			request.AddParameter("OutgoingCallerIdSid", outgoingCallerIdSid, ParameterType.UrlSegment);
			request.AddParameter("FriendlyName", friendlyName);

			return Execute<OutgoingCallerId>(request);
		}

		/// <summary>
		/// Remove a validated outgoing caller ID from the current account.
		/// Makes a DELETE request to an OutgoingCallerId Instance resource.
		/// </summary>
		/// <param name="outgoingCallerIdSid">The Sid to remove</param>
		public DeleteStatus DeleteOutgoingCallerId(string outgoingCallerIdSid)
		{
			Require.Argument("OutgoingCallerIdSid", outgoingCallerIdSid);
			var request = new RestRequest(Method.DELETE);
			request.Resource = "Accounts/{AccountSid}/OutgoingCallerIds/{OutgoingCallerIdSid}.json";

			request.AddParameter("OutgoingCallerIdSid", outgoingCallerIdSid, ParameterType.UrlSegment);

			var response = Execute(request);
			return response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed;
		}
	}
}