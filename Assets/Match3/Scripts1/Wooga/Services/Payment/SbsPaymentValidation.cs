using System;
using System.Collections.Generic;
using Match3.Scripts1.Wooga.Core.Network;
using Wooga.Core.Utilities;
using Match3.Scripts1.Wooga.Services.Authentication;
using Wooga.Foundation.Json;
using Wooga.Core.Extensions;
using Wooga.Coroutines;

namespace Match3.Scripts1.Wooga.Services.Payment
{
	// Token: 0x0200042D RID: 1069
	public class SbsPaymentValidation : ISbsPaymentValidation
	{
		// Token: 0x06001F53 RID: 8019 RVA: 0x00083507 File Offset: 0x00081907
		public SbsPaymentValidation(ISbsNetworking networking, SbsAuthentication authentication)
		{
			this._networking = networking;
			this._authentication = authentication;
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x00083520 File Offset: 0x00081920
		public IEnumerator<PaymentValidationResult> ValidateAppStoreReceipt(string receiptInBase64, Dictionary<string, object> trackingData, string transactionId, string bundleId)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{
					"payment",
					new Dictionary<string, object>
					{
						{
							"receipt",
							receiptInBase64
						},
						{
							"transaction_id",
							transactionId
						},
						{
							"bid",
							bundleId
						}
					}
				}
			};
			foreach (KeyValuePair<string, object> keyValuePair in trackingData)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return this.Validate(dictionary, PaymentValidationStores.ios, this._authentication.GetUserContext());
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x000835D8 File Offset: 0x000819D8
		public IEnumerator<PaymentValidationResult> ValidateGooglePlayReceipt(string receiptInBase64, string signature, Dictionary<string, object> trackingData)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{
					"payment",
					new Dictionary<string, object>
					{
						{
							"inapp_signed_data",
							receiptInBase64
						},
						{
							"inapp_signature",
							signature
						}
					}
				}
			};
			foreach (KeyValuePair<string, object> keyValuePair in trackingData)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return this.Validate(dictionary, PaymentValidationStores.google, this._authentication.GetUserContext());
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x00083684 File Offset: 0x00081A84
		private IEnumerator<PaymentValidationResult> Validate(Dictionary<string, object> body, PaymentValidationStores store, UserContext userContext)
		{
			SbsRequest sbsRequest = SbsRequestFactory.PaymentValidation.CreatePaymentValidateRequest(userContext, body, store);
			return this._networking.Send(sbsRequest).ContinueWith(delegate(SbsResponse response)
			{
				PaymentValidationResult result = PaymentValidationResult.Success;
				if (response.StatusCode.Is(400))
				{
					try
					{
						if (this.PaymentAlreadyValidated(response))
						{
							return PaymentValidationResult.AlreadyValidated;
						}
					}
					catch (Exception ex)
					{
						Log.Error(new object[]
						{
							"Error validating payment - " + ex.Message
						});
					}
					return PaymentValidationResult.Failure;
				}
				if (response.HasError())
				{
					result = PaymentValidationResult.Error;
				}
				return result;
			});
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x000836BC File Offset: 0x00081ABC
		private bool PaymentAlreadyValidated(SbsResponse response)
		{
			string @string = ((JSONNode)response.ParseBody()).GetString("error", null);
			return @string == "payment_already_validated" || @string == "transaction_id_already_validated";
		}

		// Token: 0x04004ADE RID: 19166
		private readonly ISbsNetworking _networking;

		// Token: 0x04004ADF RID: 19167
		private readonly SbsAuthentication _authentication;
	}
}
