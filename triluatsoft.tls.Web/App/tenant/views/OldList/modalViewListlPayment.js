(function () {
    appModule.controller('tenant.views.OldList.modalViewListlPayment', [
        '$scope',
        '$uibModalInstance',
        'abp.services.app.payment',
        '$uibModal',        
        'claimID',
        function (
            $scope,
            $uibModalInstance,
            paymentService,
            $uibModal,            
            claimID
            ) {
            var vm = this;
            vm.list = [];

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            vm.getAll = function () {
                //console.log('vm.getall');
                paymentService.search({ claimID: claimID })
                    .then(function (result) {
                        console.log('vm.getall', result.data);
                        vm.list = result.data.items;
                    });
            };

            function init() {
                vm.getAll();
            }
            init();

        }
    ]);
})();