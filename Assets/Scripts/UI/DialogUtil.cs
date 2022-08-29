using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAS.UI;
using System;

namespace SAS.UI
{
	public class DialogUtil : BaseSingleton<DialogUtil>
	{
		private bool m_surpressNextConfirmPlan = false;

		public void Welcome()
		{
            //DialogCanvas.Show("Welcome",
            //    "Welcome to Random Team Builder!",
            //    Accent.None,
            //    "Start Tour",
            //    "No",
            //    TourUtil.CastInstance.StartTour);
        }

		//private void ShowApproveReject()
		//{
		//	DialogCanvas.Show("Reject",
		//		"Stryker has received your rejection.",
		//		Accent.Correct,
		//		"Continue");
		//}
	}
}
