using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.Payment
{
	// Token: 0x0200042E RID: 1070
	public interface ISbsPaymentValidation
	{
		// Token: 0x06001F59 RID: 8025
		IEnumerator<PaymentValidationResult> ValidateAppStoreReceipt(string receiptInBase64, Dictionary<string, object> trackingData, string transactionId, string bundleId);

		// Token: 0x06001F5A RID: 8026
		IEnumerator<PaymentValidationResult> ValidateGooglePlayReceipt(string receiptInBase64, string signature, Dictionary<string, object> trackingData);
	}
}
