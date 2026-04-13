(function () {
    appModule.controller('tenant.views.OldList.modalIndexCreateInvoice', [
        '$scope',
        '$uibModalInstance',
        '$uibModal',
        'abp.services.app.claim',
        'abp.services.app.customer',
        'input',
        function (
            $scope,
            $uibModalInstance,
            $uibModal,
            claimService,
            customerService,
            input
        ) {
            var vm = this;

            vm.saving = false;
            vm.item = {};
            

            vm.cancel = function () {
                $uibModalInstance.close();
            };

            vm.selectClaim = function () {
                console.log('select claim', vm.claimID);
                customerService.getLstCustomerByClaimID(vm.claimID)
                    .then(function (result) {
                        //console.log('list customers', result.data);
                        vm.listCustomers = result.data;
                    })
            }
            vm.selectCustomer = function () {
                console.log('select customer', vm.customerID);
                let customerName = $.grep(vm.listCustomers, function (e) { return e.customerID == vm.customerID; })[0].customerName;
                console.log('select customer', customerName);
                vm.customerName = customerName;
            }
            vm.getAll = function () {
                vm.loading = true;
                if (input != null) {
                    vm.claimID = input;
                    vm.selectClaim(vm.claimID);
                }
                claimService.getOpenClaimToCreateInvoice()
                    .then(function (result) {
                        //console.log('list claim', result.data);
                        vm.listClaims = result.data;
                    })
                    .finally(function () {
                        vm.loading = false;
                    });
            };

            function init() {
                vm.getAll();
            }
            init();

           //Open modal Interim
            vm.openModalInterim = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalCreateInterim.cshtml',
                    controller: 'tenant.views.OldList.modalCreateInterim as vm',
                    backdrop: 'static',
                    size: 'lg',
                    scope: $scope
                });
                modalInstance.result.then(function (result) {
                    console.log('close create interim', $scope.$parent);
                    $scope.$parent.vm.getAll();
                    $uibModalInstance.close();
                });
            }
            //Open modal Final
            vm.openModalFinal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalCreateFinal.cshtml',
                    controller: 'tenant.views.OldList.modalCreateFinal as vm',
                    backdrop: 'static',
                    size: 'lg',
                    scope: $scope
                });
                modalInstance.result.then(function (result) {
                    $scope.$parent.vm.getAll();
                    $uibModalInstance.close();
                });
            }
        }
    ]);
})();