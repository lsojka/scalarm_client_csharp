using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using System.Linq;
using RestSharp.Deserializers;

namespace Scalarm
{


	public class SupervisedExperiment : Experiment
	{
		// TODO
//		public void SchedulePoints(List<ValuesMap> points)
//		{
//		}

//		public static implicit operator SupervisedExperiment(Experiment experiment)
//		{
//			// TODO
//			return new SupervisedExperiment();
//		}

		#region model

		// inherit model from Experiment

		[DeserializeAs(Name = "completed")]
		public bool IsCompleted {get; private set;}

		[DeserializeAs(Name = "result")]
		public string Result { get; private set;}

		#endregion


		public void SchedulePoint(ValuesMap point)
		{
			var request = new RestRequest(String.Format("experiments/{0}/schedule_point", this.Id), Method.POST);
			request.AddParameter("point", point.ToJson());
			var result = Client.Execute<SchedulePointResult> (request);
			HandleSchedulePointResponse(result);
		}

		public void SchedulePoints(IEnumerable<ValuesMap> points)
		{
			// TODO: implement multi-point scheduling in Scalarm

			foreach (ValuesMap point in points) {
				SchedulePoint(point);
			}
		}

		private void HandleSchedulePointResponse(IRestResponse<SchedulePointResult> response)
		{
			Client.ValidateResponseStatus(response);

			var dataResult = response.Data;

			if (dataResult.status == "ok")
			{
				return;
			} else if (dataResult.status == "error") {
				throw new SchedulePointException("");
			} else {
				throw new InvalidResponseException(response);
			}
		}

		// TODO: values parameter (point) support
		public void MarkAsComplete(string results, bool success = true, string errorReason = null)
		{
			var request = new RestRequest(String.Format("experiments/{0}/mark_as_complete", this.Id), Method.POST);
			request.AddParameter("status", success ? "ok" : "error");
			request.AddParameter("results", results);
			if (errorReason != null) {
				request.AddParameter("reason", errorReason);
			}

			var result = Client.Execute<ScalarmStatus>(request);

			Client.ValidateResponseStatus(result);
			if (result.Data.status == "ok") {
				return;
			} else {
				// TODO: use ScalarmException
				throw new Exception("Invalid experiment mark as complete result");
			}
		}
	}
}

