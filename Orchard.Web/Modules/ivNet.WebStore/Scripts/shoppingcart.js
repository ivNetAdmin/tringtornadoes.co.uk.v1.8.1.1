var ivNetShoppingCart = angular.module("ivNet.ShoppingCart.App", []);

ivNetShoppingCart.controller('ShoppingCartController', function ($scope, $http) {
    init();  

    function init() {
        $('div.cart-message').hide();
        $('div.cart-detail').hide();

        $scope.cartUrl = '/api/store/shoppingcart';

        setTimeout(function() {
            $http.get($scope.cartUrl)
                .success(function(data) {
                    //$scope.total = data.Total;                   
                    //$scope.vat = data.Vat;
                    //$scope.itemcount = data.ItemCount;
                    $scope.shopitems = data.ShopItems;
                    if ($scope.shopitems.length > 0) {                        
                        $("div.cart-detail").show();
                    } else {
                        $("div.cart-message").show();
                    }
                    $scope.calculate();
                })
                .error(function (data) {
                });
        }, 100);
    }

    $scope.calculate = function () {
        var subtotal = 0;
        var itemCount = 0;
        for (var i = 0; i < $scope.shopitems.length; i++) {
            var item = $scope.shopitems[i];
            subtotal += (item.Price * item.Quantity);
            itemCount += item.Quantity;
        }

        $scope.itemcount = itemCount;
        $scope.vat = subtotal * .2;
        $scope.subtotal = subtotal;
        $scope.total = $scope.vat + $scope.subtotal;             
    };

    $scope.removeItem = function(prd) {
        var quantity = $("input[name=prd" + prd + "]").val();
        if (quantity > 0) {
            $("input[name=prd" + prd + "]").val(quantity - 1);
            $("input[name=prd" + prd + "]").trigger('change');
        } else {
            $("input[name=prd" + prd + "]").val(0);
        }
    };

    $scope.saveOrder = function(createOrderUrl) {
        setTimeout(function() {
            $http.get(createOrderUrl)
                .success(function (data) {
         
                    $('input[name="invoice"]').val(JSON.parse(data));
                    $('form[name="_xclick"]').submit();
                })
                .error(function(data) {
                });
        }, 100);
    };
});