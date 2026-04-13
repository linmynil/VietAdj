using System.Collections.Generic;
using Abp.Application.Services.Dto;
using triluatsoft.tls.Editions.Dto;

namespace triluatsoft.tls.MultiTenancy.Dto
{
    public class GetTenantFeaturesForEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}