using ivNet.Webstore.Models;

namespace ivNet.Webstore.ViewModels {
    public class EditOrderVM {

        public int Id { get; set; }
        public OrderStatus Status { get; set; }

        public EditOrderVM() {
        }

        public EditOrderVM(OrderRecord order) {
            Id = order.Id;
            Status = order.Status;
        }
    }
}