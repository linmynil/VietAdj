(function () {
    appModule.controller('tenant.views.OldList.modalCreateIncome', [
        '$scope',
        '$uibModalInstance',
        'item',
        'mode',
        'recordType',
        'abp.services.app.employee',
        'abp.services.app.employeeIncome',
        function (
            $scope,
            $uibModalInstance,
            item,
            mode,
            recordType,
            employeeService,
            employeeIncomeService
        ) {
            var vm = this;

            vm.saving = false;            
            vm.editing = mode === 'edit' ? true : false;
            vm.postdata = {};
            vm.listEmp = {};
            //vm.empl = 2;
            vm.edititem = {};            
            if (item) {                
                vm.edititem = item;
                data = vm.edititem.id;
            }
            vm.save = function () {
                  abp.notify.info(app.localize('SavedSuccessfully'));
                  $uibModalInstance.close();
            };

            vm.getEmployees = function () {
                employeeService.getEmployeesForClaim({})
                    .then(function (result) {
                        vm.listEmp = result.data;
                        console.log('Employee list: ', vm.listEmp);
                    });
            };

            vm.getTypeList = function () {
                employeeIncomeService.getAllIncomeType()
                    .then(function (result) {
                        vm.typelist = result.data;
                        console.log('Type list: ', vm.typelist);
                    });
            };


            vm.loadData = function () {                                
                employeeIncomeService.getById(data)
                .then(function (result) {
                    vm.edititem = result.data[0];                    
                    if (vm.edititem.date) {
                        var startDate = moment(vm.edititem.date);
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
                vm.edititem.date = $('#createDateP').val() !== '' ? moment($('#createDateP').val(), "DD/MM/YYYY HH:mm").format() : undefined;
                console.log(vm.edititem);
                employeeIncomeService.saveEmployeeIncome(vm.edititem)
                    .then(function (result) {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                    })
                    .finally(function () {
                    });
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

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {
                if (vm.edititem.id) {
                    vm.getEmployees();
                    vm.getTypeList();
                    vm.loadData();
                } else {
                    vm.getEmployees();
                    vm.getTypeList();
                }                
            }

            init();
        }
    ]);
})();