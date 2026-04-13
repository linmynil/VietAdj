(function () {
    appModule.controller('tenant.views.OldList.viewDeadLine', [
        '$scope',
        '$uibModal',
        'abp.services.app.task',
        function (
            $scope,
            $uibModal,
            taskService
            ) {
            var vm = this;
            vm.list = [];
            vm.page = 1;

            vm.grant_search = false;
            vm.grant_create = false;
            vm.grant_edit = false;
            vm.grant_delete = false;

            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');
                $('#from').daterangepicker({
                    singleDatePicker: true,
                    autoUpdateInput: false,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                });
                $('#from').on('apply.daterangepicker', function (ev, picker) {
                    $('#from').val(picker.startDate.format('DD-MM-YYYY'));
                });


                $('#to').daterangepicker({
                    singleDatePicker: true,
                    autoUpdateInput: false,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                });
                $('#to').on('apply.daterangepicker', function (ev, picker) {
                    $('#to').val(picker.startDate.format('DD-MM-YYYY'));
                });
            });

            vm.check_create_permission = function () {
                console.log('Check permission for create deadline');
                taskService.checkPermission_CreateDeadline()
                .then(function (result) {
                    vm.grant_create = result.data;
                    console.log('Create Permission', vm.grant_create);
                    if (!vm.grant_create) {
                        document.getElementById("createbnt").disabled = true;
                    } else {
                        document.getElementById("createbnt").disabled = false;
                    }
                });
            };

            vm.check_search_permission = function () {
                console.log('Check permission for search deadline');
                taskService.checkPermission_SearchDeadline()
                .then(function (result) {
                    vm.grant_search = result.data;
                    console.log('Search Permission', vm.grant_search);
                });
            };

            vm.check_edit_permission = function () {
                console.log('Check permission for edit deadline');
                taskService.checkPermission_EditDeadline()
                .then(function (result) {
                    vm.grant_edit = result.data;
                    console.log('Edit Permission', vm.grant_edit);
                });
            };

            vm.check_delete_permission = function () {
                console.log('Check permission for delete deadline');
                taskService.checkPermission_DeleteDeadline()
                .then(function (result) {
                    vm.grant_delete = result.data;
                    console.log('Delete Permission', vm.grant_edit);
                });
            };

            vm.getAll = function () {
                try {
                    console.log('vm.from >>>>>>>', $('#from').val());
                    let from = $('#from').val() !== '' ? moment($('#from').val(), "DD-MM-YYYY").format() : undefined;
                    let to = $('#to').val() !== '' ? moment($('#to').val(), "DD-MM-YYYY").format() : undefined;
                    console.log('getAll', $.extend({}, vm.searchdata, { startDate: from }, { endDate: to }));
                    vm.loading = true;
                    taskService.getAll($.extend({}, vm.searchdata, { startDate: from }, { endDate: to }))
                        .then(function (result) {
                            vm.list = result.data;
                            console.log('list', vm.list);
                        })
                        .finally(function () {
                            vm.loading = false;
                        });
                } catch (e) {
                    console.log(e);
                }
            };
            vm.search = function (event) {
                event.preventDefault();
                vm.getAll();
            }

            vm.pageChanged = function () {
                vm.getAll();
            };

            vm.searchLogs = function (event) {
                try {
                    vm.getAll();
                } catch (e) {
                    console.log(e)
                } finally {
                    event.preventDefault();
                }
            }

            vm.init = function () {
                vm.check_create_permission();
                vm.check_search_permission();
                vm.check_edit_permission();
                vm.check_delete_permission();
                vm.getAll();
            }

            vm.init();

            vm.reset = function () {
                console.log('vm.reset');
                vm.searchdata = {};
                vm.dateRangeModel = {};
                vm.page = 1;
            }

            vm.openModal = function (itemIn, mode) {
                console.log('open modalCreateDeadline', itemIn, mode);
                vm.item = itemIn;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalCreateDeadline.cshtml',
                    controller: 'tenant.views.OldList.modalCreateDeadline as vm',
                    backdrop: 'static',
                    scope: $scope,
                    resolve: {
                        item: function () {
                            return vm.item;
                        }
                    }
                });
                modalInstance.result.then(function () {
                });
            };


            vm.delete = function (taskId) {
                abp.message.confirm('AreYouSureDeleteInformation?',
                    function (isConfirmed) {
                        if (isConfirmed) {
                            console.log('delete taskId', taskId);
                            taskService.deleteById(taskId)
                                .then(function (result) {
                                    console.log('delete taskId result', result);
                                    if (result.data == 'ok') {
                                        abp.notify.info('The task has been deleted successfully');
                                    }
                                })
                                .then(function () {
                                    vm.getAll();
                                });
                        }
                    });
            }

        }
    ]);
})();