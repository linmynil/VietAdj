(function () {
    appModule.controller('tenant.views.OldList.modalIssue', [
        '$scope', '$uibModalInstance',
        'timesheetName',
        'timesheetId',
        'abp.services.app.timeSheet',
        function ($scope, $uibModalInstance,
            timesheetName,
            timesheetId,
            timesheetService) {
            var vm = this;

            vm.saving = false;
            vm.item = {};
            vm.timesheetId = timesheetId;
            vm.timesheetName = timesheetName;

            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');
                $('input[name="issueDate"]').daterangepicker({
                    singleDatePicker: true,
                    locale: {
                        format: 'DD/MM/YYYY'
                    }
                });
            });
            vm.page = 1;

            vm.save = function () {
                var issueDate = new moment(vm.item.issueDate, 'DD/MM/YYYY').format();
                console.log('save modal issue', vm.item.issueDate, issueDate);
                vm.saving = true;
                timesheetService.issued(timesheetId, issueDate).then(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                }).finally(function () {
                    vm.saving = false;
                });
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