(function () {
    appModule.controller('tenant.views.OldList.createOrUpdateClaimModal', [
        '$scope', '$uibModalInstance', 'itemId', 'mode', 'abp.services.app.customer', 'abp.services.app.employee',
        function ($scope, $uibModalInstance, itemId, mode, customerService, employeeService) {
            var vm = this;
            console.log(itemId);
            $('#multiselect').multiselect();


            vm.getCustomers = function () {
                customerService.getByType('C')
                    .then(function (result) {
                        vm.listInsureres = result.data;
                    });
                customerService.getByType('B')
                    .then(function (result) {
                        vm.listBrokers = result.data;
                    });
            };
            vm.getEmployees = function () {
                console.log('getemployees');
                employeeService.getEmployeesForClaim({})
                    .then(function (result) {
                        vm.listEmployees = result.data;
                    });
            };
            vm.init = function () {
                //vm.getCustomers();
                //vm.getEmployees();
            }

            vm.init();
            vm.save = function () {
                console.log('create');
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {

            }

            init();
        }
    ]);
})();