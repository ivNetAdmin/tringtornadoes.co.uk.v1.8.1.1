using Orchard.Events;

namespace ivNet.Webstore.Extensibility {
    public interface IPaymentServiceProvider : IEventHandler {
        void RequestPayment(PaymentRequest e);
        void ProcessResponse(PaymentResponse e);
    }
}