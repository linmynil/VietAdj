(function () {
    appModule.controller('tenant.views.OldList.followUp', [
        '$scope', '$uibModal','abp.services.app.followUp',
        function ($scope, $uibModal, followUpService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.list = [];

            vm.getAll = function () {
                followUpService.getAll({})
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
                            followUpService.delete(item.id).then(function () {
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                                vm.getAll();
                            });
                        }
                    }
                );
            };
            
            vm.openModal = function (itemIn) {
                console.log('open createOrUpdateFollowUpModal', itemIn);
                vm.item = itemIn;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/createOrUpdateFollowUpModal.cshtml',
                    controller: 'tenant.views.OldList.createOrUpdateFollowUpModal as vm',
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