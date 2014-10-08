var ivNetShoppingCart = angular.module("ivNet.ShoppingCart.App", []);

ivNetShoppingCart.controller('ShoppingCartController', function ($scope, $http) {
    init();

    function init() {
        $scope.url = '/api/store/shoppingcart';

        setTimeout(function() {
            $http.get($scope.url)
                .success(function(data) {
                    $scope.total = data.Total;
                    $scope.subtotal = data.Subtotal;
                    $scope.vat = data.Vat;
                    $scope.itemcount = data.ItemCount;
                    $scope.shopitems = data.ShopItems;

                })
                .error(function (data) {
                });
        }, 100);
    }
});