using System;
using Newtonsoft.Json;

namespace Ditch.BitShares.Models
{
    /// <summary>
    /// account_history_operation_detail
    /// libraries\wallet\include\graphene\wallet\wallet.hpp
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class AccountHistoryOperationDetail
    {

        /// <summary>
        /// API name: total_count
        /// = 0;
        /// </summary>
        /// <returns>API type: uint32_t</returns>
        [JsonProperty("total_count")]
        public UInt32 TotalCount { get; set; }

        /// <summary>
        /// API name: result_count
        /// = 0;
        /// </summary>
        /// <returns>API type: uint32_t</returns>
        [JsonProperty("result_count")]
        public UInt32 ResultCount { get; set; }

        /// <summary>
        /// API name: details
        /// 
        /// </summary>
        /// <returns>API type: operation_detail_ex</returns>
        [JsonProperty("details")]
        public OperationDetailEx[] Details { get; set; }
    }
}
