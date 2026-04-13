(function () {
    appModule.controller('tenant.views.OldList.modalExpenses', [
        '$scope',
        '$uibModalInstance',
        'item',
        'timesheetName',
        'timesheetId',
        'abp.services.app.timeSheet',
        'abp.services.app.expenseType',
        function (
            $scope,
            $uibModalInstance,
            item,
            timesheetName,
            timesheetId,
            timesheetService,
            expenseTypeService
        ) {
            var vm = this;
            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');

                //mapping inputdate in case edit
                if (vm.item && vm.item.inputDate) {
                    let date = moment(vm.item.inputDate);
                    $('#date').daterangepicker({
                        singleDatePicker: true,
                        startDate: date.format('DD-MM-YYYY'),
                        locale: {
                            format: 'DD-MM-YYYY'
                        },
                    });
                }
            });

            vm.timesheetName = timesheetName;
            vm.item = {};
            if (item) {
                vm.item = item;
            }
            vm.item.timeSheetID = timesheetId;
            vm.save = function () {
                vm.item.inputDate = moment(document.getElementById("date").value, ["DD-MM-YYYY"]).format();
                console.log('save expense', vm.item.inputDate);
                var postdata = $.extend({}, vm.item);
                console.log('save', postdata);
                vm.saving = true;
                timesheetService.saveExpense(postdata)
                    .then(function (result) {
                        if (result.data == 'ok') {
                            abp.notify.info('Update expense successfully');
                        }
                        $uibModalInstance.close();
                        return result.data;
                    }).then(function (result) {
                        console.log('save done', $scope.$parent);
                        $scope.$parent.vm.getAll();
                    }).finally(function () {
                        vm.saving = false;
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.close();
            };

            vm.getAll = function () {
                expenseTypeService.getAll({})
                    .then(function (result) {
                        vm.listTypes = result.data;
                        console.log('listTypes', vm.listTypes);
                    });
            };
            function init() {

                vm.getAll();
            }

            init();

            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');

                console.log('Start timesheetService.getTSInfo result');
                timesheetService.getTSInfo(timesheetId)
                .then(function (result) {
                    vm.timesheet = result.data;
                    console.log('timesheetService.getTSInfo result', vm.timesheet);
                    //var mind = new Date(vm.timesheet.createdDate);
                    var mind = new Date('2000-01-01');
                    console.log('min date', mind);
                    var maxd = new Date();
                    maxd.setDate(maxd.getDate() + 365);
                    console.log('max date', maxd);

                    $('#date').daterangepicker({
                        singleDatePicker: true,
                        autoUpdateInput: false,
                        minDate: mind,
                        maxDate: maxd,
                        locale: {
                            format: 'DD-MM-YYYY'
                        },
                    });
                    $('#date').on('apply.daterangepicker', function (ev, picker) {
                        $('#date').val(picker.startDate.format('DD-MM-YYYY'));
                    });
                });
            });

            //END
        }
    ]);
})();