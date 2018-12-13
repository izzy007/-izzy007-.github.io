angular.module('myApp', [])
    .controller('myCtrl', ['$scope', function ($scope) {
        $scope.isDisabled = false;
        if ($scope.isDisabled === false) {
            $scope.PayNow = function () {
                $scope.isDisabled = true;
            }
        }
    }]);