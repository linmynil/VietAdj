(function () {
    appModule.controller('tenant.views.OldList.borderauxStatus', [
        '$scope', '$uibModal','abp.services.app.borderauxStatus',
        function ($scope, $uibModal, borderauxStatusService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.list = [];

            vm.getAll = function () {
                borderauxStatusService.getAll({})
                    .then(function (result) {
                        vm.list = result.data;
                        console.log('list', vm.list);
                    });
            };
            vm.getAll();

            vm.delete = function (item) {
                abp.message.confirm(
                    '',
                    function (isConfirmed) {
                        if (isConfirmed) {
                            borderauxStatusService.delete(item.id).then(function () {
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                                vm.getAll();
                            });
                        }
                    }
                );
            };
            
            vm.openModal = function (itemIn) {
                console.log('open createOrUpdateBorderauxStatusModal', itemIn);
                vm.item = itemIn;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/createOrUpdateBorderauxStatusModal.cshtml',
                    controller: 'tenant.views.OldList.createOrUpdateBorderauxStatusModal as vm',
                    backdrop: 'static',
                    resolve: {
                        item: function () {
                            return vm.item;
                        }
                    }
                });

                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };

        }
    ]);
})();