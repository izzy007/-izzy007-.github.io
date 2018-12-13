        angular.module('PopupDemo', ['ui.bootstrap']);
        angular.module('PopupDemo').controller('PopupDemoCont', ['$scope','$modal',function ($scope, $modal) {
            $scope.open = function () {
                var modalInstance = $modal.open({
					  controller: 'PopupCont',
                    templateUrl: 'Popup.html',
                });
            }
        }]);

        angular.module('PopupDemo').controller('PopupCont', ['$scope','$modalInstance',function ($scope, $modalInstance) {
            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }]);