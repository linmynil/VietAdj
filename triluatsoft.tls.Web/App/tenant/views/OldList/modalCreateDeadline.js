(function () {
    appModule.controller('tenant.views.OldList.modalCreateDeadline', [
        '$scope',
        '$uibModalInstance',
        'item',
        'abp.services.app.claim',
        'abp.services.app.taskName',
        'abp.services.app.task',
        function (
            $scope,
            $uibModalInstance,
            item,
            claimService,
            taskNameService,
            taskService
        ) {
            var vm = this;

            vm.saving = false;
            vm.item = {};
            if (item) {
                vm.item = item;
            }
            console.log('vm.item',vm.item);
            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');


                if (vm.item.startDate) {
                    var startDate = moment(vm.item.startDate);
                    $('#start').daterangepicker({
                        singleDatePicker: true,
                        timePicker: true,
                        timePicker24Hour: true,
                        autoApply: true,
                        autoUpdateInput: true,
                        startDate: startDate.format('HH:mm DD-MM-YYYY'),
                        locale: {
                            format: 'HH:mm DD-MM-YYYY'
                        }
                    });
                } else {
                    $('#start').daterangepicker({
                        singleDatePicker: true,
                        timePicker: true,
                        timePicker24Hour: true,
                        autoApply: true,
                        autoUpdateInput: true,
                        //startDate: moment().startOf('day'),
                        startDate: moment().add(7, 'hours').format('HH:mm DD-MM-YYYY'),
                        locale: {
                            format: 'HH:mm DD-MM-YYYY'
                        }
                    });

                }

                $('#start').on('apply.daterangepicker', function (ev, picker) {
                    $('#start').val(picker.startDate.format('HH:mm DD-MM-YYYY'));
                });
                

                if (vm.item.endDate) {
                    var endDate = moment(vm.item.endDate);
                    $('#end').daterangepicker({
                        singleDatePicker: true,
                        timePicker: true,
                        timePicker24Hour: true,
                        autoApply: true,
                        autoUpdateInput: true,
                        startDate: endDate.format('HH:mm DD-MM-YYYY'),
                        locale: {
                            format: 'HH:mm DD-MM-YYYY'
                        }
                    });
                } else {
                    $('#end').daterangepicker({
                        singleDatePicker: true,
                        timePicker: true,
                        timePicker24Hour: true,
                        autoApply: true,
                        autoUpdateInput: false,
                        startDate: moment().startOf('day'),
                        locale: {
                            format: 'HH:mm DD-MM-YYYY'
                        }
                    });
                }
                $('#end').on('apply.daterangepicker', function (ev, picker) {
                    $('#end').val(picker.startDate.format('HH:mm DD-MM-YYYY'));
                });
            });

          
            vm.save = function () {
                console.log('$scope.$parent', $scope.$parent);
                let parent = $scope.$parent;
                let from = $('#start').val() !== '' ? moment($('#start').val(), "HH:mm DD-MM-YYYY").format() : undefined;
                let to = $('#end').val() !== '' ? moment($('#end').val(), "HH:mm DD-MM-YYYY").format() : undefined;
                var postdata = $.extend({}, vm.item, { startDate: from }, { endDate: to });
                console.log('save', postdata);
                taskService.save(postdata)
                    .then(function (result) {
                        console.log('save result', result.data);
                        if (result.data == 'ok') {
                            abp.notify.info('The task has been saved successfully');
                        }
                        parent.vm.getAll();
                    })
                $uibModalInstance.close();
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            vm.getClaims = function () {
                vm.loading = true;
                claimService.getOpenClaimToCreateTask()
                    .then(function (result) {
                        vm.listClaims = result.data;
                    })
                    .finally(function () {
                        vm.loading = false;
                    });
            };
            vm.selectClaim = function () {
                console.log('select claim', vm.item.claimID);
                if (vm.item.claimID) {
                    claimService.getEmployeeByClaim(vm.item.claimID)
                        .then(function (result) {
                            console.log('employee', result.data);
                            vm.listEmployees = result.data;
                        });
                }
            };
            vm.getTasks = function () {
                taskNameService.getAll()
                    .then(function (result) {
                        //console.log('getTasks', result.data);
                        vm.listTasks = result.data;
                    });
            };

            vm.getTaskById = function () {
                try {
                    console.log('vm.gettaskbyid', vm.item.id);
                    if (vm.item.id) {
                        taskService.getById(vm.item.id)
                            .then(function (result) {
                                console.log('getbyid result', result.data);
                                vm.item = result.data;
                            })
                    }
                } catch (e) {
                    console.log('eee', e);
                }
            }

            vm.selectTask = function () {
                console.log('select task', vm.item.taskNameID);
                var temp = $.grep(vm.listTasks, function (e) { return e.id == vm.item.taskNameID; })[0];
                vm.item.description = temp.name;
            }

            function init() {
                console.log('init....');
                vm.getClaims();
                vm.getTasks();

                //in edit mode
                if (vm.item.id) {
                    vm.getTaskById();
                    vm.selectClaim();
                }
                //
                console.log('init done');
            }

            init();
        }
    ]);
})();