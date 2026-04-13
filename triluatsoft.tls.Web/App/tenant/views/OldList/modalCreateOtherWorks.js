(function () {
    appModule.controller('tenant.views.OldList.modalCreateOtherWorks', [
        '$scope',
        '$uibModalInstance',
        'item',
        'abp.services.app.employee',
        'abp.services.app.contributionAdjustment',
        function (
            $scope,
            $uibModalInstance,
            item,
            employeeService,
            contributionAdjustmentService
        ) {
            var vm = this;

            vm.saving = false;
            vm.item = {};
            vm.added = true;
            if (item) {
                vm.item = item;
                data = vm.item.id;
            }
            vm.getEmployees = function () {
                console.log('getemployees');
                employeeService.getEmployeesForClaim({})
                    .then(function (result) {
                        vm.listEmployees = result.data;
                    });
            };

            vm.getAll = function () {
                vm.loading = true;                
                console.log('Other Work.get Id :', vm.item);
                contributionAdjustmentService.getById(data)
                    .then(function (result) {
                        vm.item = result.data;
                        if (vm.item.adjustAMT < 0) {
                            vm.added = false;
                            vm.item.adjustAMT = vm.item.adjustAMT * (-1);
                        }
                        if (vm.item.adjustDate) {
                            var startDate = moment(vm.item.adjustDate);
                            $('#createDateP').daterangepicker({
                                singleDatePicker: true,
                                timePicker: true,
                                timePicker24Hour: true,
                                autoApply: true,
                                autoUpdateInput: true,
                                startDate: startDate.format('DD/MM/YYYY HH:mm'),
                                locale: {
                                    format: 'DD/MM/YYYY HH:mm'
                                }
                            });
                        } else {
                            $('#createDateP').daterangepicker({
                                singleDatePicker: true,
                                timePicker: true,
                                timePicker24Hour: true,
                                autoApply: true,
                                autoUpdateInput: false,
                                startDate: moment().startOf('day'),
                                locale: {
                                    format: 'DD/MM/YYYY HH:mm'
                                }
                            });

                        }
                    })
                    .finally(function () {
                        vm.loading = false;
                    });
            };

            vm.save = function () {                
                vm.item.adjustDate = $('#createDateP').val() !== '' ? moment($('#createDateP').val(), "DD/MM/YYYY HH:mm").format() : undefined;                
                if (!vm.added) {
                    vm.item.adjustAMT = vm.item.adjustAMT * (-1);
                }
                console.log(vm.item);
                contributionAdjustmentService.save(vm.item)
                    .then(function (result) {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                    })
                    .finally(function () {
                    });
                    
            };

            vm.cancel = function () {
                $uibModalInstance.close();
            };

            //control date of survey
            $('#createDateP').daterangepicker({
                singleDatePicker: true,
                autoUpdateInput: false,
                locale: {
                    format: 'DD/MM/YYYY HH:mm'
                },
            });
            $('#createDateP').on('apply.daterangepicker', function (ev, picker) {
                $('#createDateP').val(picker.startDate.format('DD/MM/YYYY HH:mm'));
            });
            //end control date of survey

            onchange = function () {                
                if (vm.item.true == "true")
                    vm.item.adjustAMT = '-' + vm.item.adjustAMT;
                else
                    vm.item.adjustAMT = vm.item.adjustAMT;
            }

            function init() {
                if (vm.item.id) {
                    vm.getEmployees();
                    vm.getAll();
                }
                vm.getEmployees();
            }

            init();
        }
    ]);
})();