using SLS_LegalServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.Helpers
{
    public class CommonHelper
    {
        public static bool IsCaseReferralChecked(IntakeVM model, int referralId)
        {
            bool result = false;

            if (model != null)
            {
                if (model.ReferralSources != null)
                    result = model.ReferralSources.Any(r => r.ReferralSourceId == referralId);
            }

            return result;
        }
    }
}