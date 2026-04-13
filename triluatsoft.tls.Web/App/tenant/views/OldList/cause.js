(function () {
    appModule.controller('tenant.views.OldList.cause', [
        '$scope', '$uibModal','abp.services.app.cause',
        function ($scope, $uibModal, causeService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.causes = [];

            vm.getAll = function () {
                causeService.getAll({})
                    .then(function (result) {
                        vm.causes = result.data;
                        console.log('causes', vm.causes);
                    });
            };
            vm.getAll();

            vm.delete = function (item) {
                abp.message.confirm(
                    '',
                    function (isConfirmed) {
                        if (isConfirmed) {
                            causeService.delete(item.id).then(function () {
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                                vm.getAll();
                            });
                        }
                    }
                );
            };

            vm.openCreateModal = function () {
                console.log('open create modal');
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/createCauseModal.cshtml',
                    controller: 'tenant.views.OldList.createCauseModal as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };

            vm.openEditModal = function (itemIn) {
                console.log('open edit modal');
                vm.item = itemIn;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/updateCauseModal.cshtml',
                    controller: 'tenant.views.OldList.updateCauseModal as vm',
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