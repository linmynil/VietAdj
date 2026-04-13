(function () {
    appModule.controller('tenant.views.OldList.updateClaimProcess', [
        '$scope', '$uibModal', '$window', '$location', '$stateParams', 'abp.services.app.claim', 'abp.services.app.employee', 'abp.services.app.claimProcess',
        function ($scope, $uibModal, $window, $location, $stateParams, claimService, employeeService, claimprocessService) {
            var vm = this;
            
            vm.item = {};
            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');
            });
            
            vm.getAll = function () {
                console.log('Claims process');
                if (vm.item.claimId) {
                    claimprocessService.getAll(vm.item.claimId)
                    .then(function (result) {
                        console.log('Claims process', result.data);
                        vm.item.claimps = result.data;
                    });
                } else {
                    claimprocessService.getAll('')
                    .then(function (result) {
                        console.log('Claims process', result.data);
                        vm.item.claimps = result.data;
                    });
                }
                
            };

            vm.getOpenClaims = function () {
                claimprocessService.getProcessClaim({})
                    .then(function (result) {
                        console.log('open claims', result.data);                        
                        vm.item.claims = result.data;
                        vm.item.claims.unshift("All");
                    });
            };

            vm.updateClaim = function (claimpID, statusValue) {
                console.log('Update id: ', claimpID);
                claimprocessService.updateClaimProcess(claimpID, statusValue)
                    .then(function (result) {
                        console.log('Update successfully!');
                        vm.getAll();
                    });
            }

            vm.selectClaimId = function () {
                console.log('select claim', vm.item.claimId);
                vm.getAll();
            };

            vm.getOpenClaims();
            vm.getAll();

            //END
        }
    ]);
})();