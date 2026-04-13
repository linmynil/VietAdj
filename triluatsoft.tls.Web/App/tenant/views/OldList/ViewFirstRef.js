(function () {
    appModule.controller('tenant.views.OldList.ViewFirstRef', [
        '$scope',
        '$uibModal',
        'abp.services.app.firstRef',
        function (
            $scope,
            $uibModal,
            firstRefService
            ) {
            var vm = this;
            vm.list = [];
            vm.activeItem = [];
            vm.getAll = function () {
                vm.loading = true;
                console.log('Get All');
                if (vm.officeID == null || vm.officeID == '') {
                    vm.officeID = '1';
                }
                firstRefService.getAll(vm.officeID)
                .then(function (result) {
                    vm.list = result.data;
                    console.log('List result', vm.list);
                    vm.loading = false;
                    vm.list.forEach(function (element) {
                        if (element.isActive) {
                            vm.activeItem = element;
                        }                        
                    });
                    console.log('Active item', vm.activeItem);
                });
            };

            vm.officeChanged = function () {
                vm.getAll();
            };

            vm.addRef = function () {                
                var err = '';                
                if (vm.firstRefVal == null || vm.firstRefVal == '') {                    
                    err = 'Please enter FirstRef';
                }
                if (vm.syearVal == null || vm.syearVal == '') {
                    err = 'Please select Year!';
                }

                if (vm.officeID == null || vm.officeID == '') {
                    vm.officeID = '1';                    
                }

                if (err == '') {
                    console.log('Add Ref');
                    var notadd = false;
                    for (i = 0; i < vm.list.length; i++) {
                        if ((vm.list[i].officeID == vm.officeID) && (vm.list[i].sYear == vm.syearVal)) {
                            notadd = true;
                        }
                    }

                    if (notadd) {
                        abp.notify.error('FirstRef is exist!');                        
                    } else {
                        firstRefService.create(vm.officeID, vm.syearVal, vm.firstRefVal)
                            .then(function () {
                                abp.notify.info(app.localize('SavedSuccessfully'));
                                vm.loading = false;
                                vm.init();
                            });
                    }
                    vm.loading = false;
                    vm.init();
                } else {
                    abp.notify.error(err);
                    vm.loading = false;
                    vm.init();
                }
                                
            }

            vm.activeChange = function (itemlist) {
                console.log('Checked list item', itemlist);
                vm.activeItem = itemlist;
            }

            vm.activeRef = function () {
                console.log('Post data', vm.activeItem);
                firstRefService.setActive(vm.activeItem)
                    .then(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        vm.loading = false;
                        vm.init();
                    });
            }

            vm.init = function () {
                vm.getAll();
            }
            vm.init();
        }
    ]);
})();