/* 'app' MODULE DEFINITION */
var appModule = angular.module("app", [
    "ui.router",
    "ui.bootstrap",
    'ui.utils',
    "ui.jq",
    'ui.grid',
    'ui.grid.pagination',
    "oc.lazyLoad",
    "ngSanitize",
    'angularFileUpload',
    'daterangepicker',
    'angularMoment',
    'frapontillo.bootstrap-switch',
    'abp'
]);

/* LAZY LOAD CONFIG */

/* This application does not define any lazy-load yet but you can use $ocLazyLoad to define and lazy-load js/css files.
 * This code configures $ocLazyLoad plug-in for this application.
 * See it's documents for more information: https://github.com/ocombe/ocLazyLoad
 */
appModule.config(['$ocLazyLoadProvider', function ($ocLazyLoadProvider) {
    $ocLazyLoadProvider.config({
        cssFilesInsertBefore: 'ng_load_plugins_before', // load the css files before a LINK element with this ID.
        debug: false,
        events: true,
        modules: []
    });
}]);

/* THEME SETTINGS */
App.setAssetsPath(abp.appPath + 'metronic/assets/');
appModule.factory('settings', ['$rootScope', function ($rootScope) {
    var settings = {
        layout: {
            pageSidebarClosed: false, // sidebar menu state
            pageContentWhite: true, // set page content layout
            pageBodySolid: false, // solid body color state
            pageAutoScrollOnLoad: 1000 // auto scroll to top on page load
        },
        layoutImgPath: App.getAssetsPath() + 'admin/layout4/img/',
        layoutCssPath: App.getAssetsPath() + 'admin/layout4/css/',
        assetsPath: abp.appPath + 'metronic/assets',
        globalPath: abp.appPath + 'metronic/assets/global',
        layoutPath: abp.appPath + 'metronic/assets/layouts/layout4'
    };

    $rootScope.settings = settings;

    return settings;
}]);

/* ROUTE DEFINITIONS */

appModule.config([
    '$stateProvider', '$urlRouterProvider', '$qProvider',
    function ($stateProvider, $urlRouterProvider, $qProvider) {

        // Default route (overrided below if user has permission)
        $urlRouterProvider.otherwise("/tenant/dashboard");

        //Welcome page
        $stateProvider.state('welcome', {
            url: '/welcome',
            templateUrl: '~/App/common/views/welcome/index.cshtml'
        });

        //COMMON routes

        if (abp.auth.hasPermission('Pages.Administration.Roles')) {
            $stateProvider.state('roles', {
                url: '/roles',
                templateUrl: '~/App/common/views/roles/index.cshtml'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Users')) {
            $stateProvider.state('users', {
                url: '/users?filterText',
                templateUrl: '~/App/common/views/users/index.cshtml'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Languages')) {
            $stateProvider.state('languages', {
                url: '/languages',
                templateUrl: '~/App/common/views/languages/index.cshtml'
            });

            if (abp.auth.hasPermission('Pages.Administration.Languages.ChangeTexts')) {
                $stateProvider.state('languageTexts', {
                    url: '/languages/texts/:languageName?sourceName&baseLanguageName&targetValueFilter&filterText',
                    templateUrl: '~/App/common/views/languages/texts.cshtml'
                });
            }
        }

        if (abp.auth.hasPermission('Pages.Administration.AuditLogs')) {
            $stateProvider.state('auditLogs', {
                url: '/auditLogs',
                templateUrl: '~/App/common/views/auditLogs/index.cshtml'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.OrganizationUnits')) {
            $stateProvider.state('organizationUnits', {
                url: '/organizationUnits',
                templateUrl: '~/App/common/views/organizationUnits/index.cshtml'
            });
        }

        $stateProvider.state('notifications', {
            url: '/notifications',
            templateUrl: '~/App/common/views/notifications/index.cshtml'
        });

        //HOST routes

        $stateProvider.state('host', {
            'abstract': true,
            url: '/host',
            template: '<div ui-view class="fade-in-up"></div>'
        });

        if (abp.auth.hasPermission('Pages.Tenants')) {
            $urlRouterProvider.otherwise("/host/tenants"); //Entrance page for the host
            $stateProvider.state('host.tenants', {
                url: '/tenants?filterText',
                templateUrl: '~/App/host/views/tenants/index.cshtml'
            });
        }

        if (abp.auth.hasPermission('Pages.Editions')) {
            $stateProvider.state('host.editions', {
                url: '/editions',
                templateUrl: '~/App/host/views/editions/index.cshtml'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Host.Maintenance')) {
            $stateProvider.state('host.maintenance', {
                url: '/maintenance',
                templateUrl: '~/App/host/views/maintenance/index.cshtml'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Host.Settings')) {
            $stateProvider.state('host.settings', {
                url: '/settings',
                templateUrl: '~/App/host/views/settings/index.cshtml'
            });
        }

        //TENANT routes

        $stateProvider.state('tenant', {
            'abstract': true,
            url: '/tenant',
            template: '<div ui-view class="fade-in-up"></div>'
        });

        if (abp.auth.hasPermission('Pages.Tenant.Dashboard')) {
            $urlRouterProvider.otherwise("/tenant/dashboard"); //Entrance page for a tenant
            $stateProvider.state('tenant.dashboard', {
                url: '/dashboard',
                templateUrl: '~/App/tenant/views/dashboard/index.cshtml'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Tenant.Settings')) {
            $stateProvider.state('tenant.settings', {
                url: '/settings',
                templateUrl: '~/App/tenant/views/settings/index.cshtml'
            });
        }

        //viet adjuster
        $stateProvider.state('tenant.cause', {
            url: '/cause',
            templateUrl: '~/App/tenant/views/OldList/cause.cshtml'
        });
        $stateProvider.state('tenant.typeOfLoss', {
            url: '/type-of-loss',
            templateUrl: '~/App/tenant/views/OldList/typeOfLoss.cshtml'
        });
        $stateProvider.state('tenant.borderauxStatus', {
            url: '/borderaux-status',
            templateUrl: '~/App/tenant/views/OldList/borderauxStatus.cshtml'
        });
        $stateProvider.state('tenant.followUp', {
            url: '/follow-up',
            templateUrl: '~/App/tenant/views/OldList/followUp.cshtml'
        });
        $stateProvider.state('tenant.expenseType', {
            url: '/expense-type',
            templateUrl: '~/App/tenant/views/OldList/expenseType.cshtml'
        });
        $stateProvider.state('tenant.claimType', {
            url: '/claim-type',
            templateUrl: '~/App/tenant/views/OldList/claimType.cshtml'
        });
        $stateProvider.state('tenant.report', {
            url: '/report',
            templateUrl: '~/App/tenant/views/OldList/report.cshtml'
        });
        $stateProvider.state('tenant.taskName', {
            url: '/task-name',
            templateUrl: '~/App/tenant/views/OldList/taskName.cshtml'
        });
        $stateProvider.state('tenant.customers', {
            url: '/customers',
            templateUrl: '~/App/tenant/views/OldList/customer.cshtml'
        });
        $stateProvider.state('tenant.claim', {
            url: '/claim',
            templateUrl: '~/App/tenant/views/OldList/claim.cshtml'
            , params: {
                claimRefId: null
            }
        });
        $stateProvider.state('tenant.cuClaim', {
            url: '/cu-claim',
            templateUrl: '~/App/tenant/views/OldList/createOrUpdateClaim.cshtml'
            , params: {
                claimId: null
            }
        });
        $stateProvider.state('tenant.updateBorderaux', {
            url: '/update-borderaux',
            templateUrl: '~/App/tenant/views/OldList/updateBorderaux.cshtml'
            
        });
        $stateProvider.state('tenant.updateClaimProcess', {
            url: '/update-claim-process',
            templateUrl: '~/App/tenant/views/OldList/updateClaimProcess.cshtml'

        });
        $stateProvider.state('tenant.viewDeadLine', {
            url: '/viewDeadLine',
            templateUrl: '~/App/tenant/views/OldList/viewDeadLine.cshtml'
        });
        $stateProvider.state('tenant.viewTimeSheet', {
            url: '/viewTimeSheet',
            templateUrl: '~/App/tenant/views/OldList/viewTimeSheet.cshtml'
        });
        $stateProvider.state('tenant.viewInvoiceManage', {
            url: '/viewInvoiceManage',
            templateUrl: '~/App/tenant/views/OldList/viewInvoiceManage.cshtml'
        });
        $stateProvider.state('tenant.viewAccountsReceivable', {
            url: '/viewAccountsReceivable',
            templateUrl: '~/App/tenant/views/OldList/viewAccountsReceivable.cshtml'
        });
        $stateProvider.state('tenant.viewVAT', {
            url: '/viewVAT',
            templateUrl: '~/App/tenant/views/OldList/viewVAT.cshtml'
        });
        $stateProvider.state('tenant.viewOtherReceiptsPayments', {
            url: '/viewOtherReceiptsPayments',
            templateUrl: '~/App/tenant/views/OldList/viewOtherReceiptsPayments.cshtml'
        });
        $stateProvider.state('tenant.viewUserContribution', {
            url: '/viewUserContribution',
            templateUrl: '~/App/tenant/views/OldList/viewUserContribution.cshtml'
        });
        $stateProvider.state('tenant.viewOtherWorks', {
            url: '/viewOtherWorks',
            templateUrl: '~/App/tenant/views/OldList/viewOtherWorks.cshtml'
        });
        $stateProvider.state('tenant.viewIncome', {
            url: '/viewIncome',
            templateUrl: '~/App/tenant/views/OldList/viewIncome.cshtml'
        });
        $stateProvider.state('tenant.viewCRS', {
            url: '/viewCRS',
            templateUrl: '~/App/tenant/views/OldList/viewCRS.cshtml'
        });

        //Report/Export
        $stateProvider.state('tenant.viewUserClaimBordereauxReport', {
            url: '/viewUserClaimBordereauxReport',
            templateUrl: '~/App/tenant/views/OldList/viewUserClaimBordereauxReport.cshtml'
        });
        $stateProvider.state('tenant.viewFeeReport', {
            url: '/viewFeeReport',
            templateUrl: '~/App/tenant/views/OldList/viewFeeReport.cshtml'
        });
        $stateProvider.state('tenant.viewExpensesReport', {
            url: '/viewExpensesReport',
            templateUrl: '~/App/tenant/views/OldList/viewExpensesReport.cshtml'
        });
        $stateProvider.state('tenant.viewMonthlyExpensesReport', {
            url: '/viewMonthlyExpensesReport',
            templateUrl: '~/App/tenant/views/OldList/viewMonthlyExpensesReport.cshtml'
        });
        $stateProvider.state('tenant.viewActualFeeReport', {
            url: '/viewActualFeeReport',
            templateUrl: '~/App/tenant/views/OldList/viewActualFeeReport.cshtml'
        });
        $stateProvider.state('tenant.viewWIPReport', {
            url: '/viewWIPReport',
            templateUrl: '~/App/tenant/views/OldList/viewWIPReport.cshtml'
        });
        $stateProvider.state('tenant.viewRevenueReport', {
            url: '/viewRevenueReport',
            templateUrl: '~/App/tenant/views/OldList/viewRevenueReport.cshtml'
        });
        $stateProvider.state('tenant.viewAccountsReceivableReport', {
            url: '/viewAccountsReceivableReport',
            templateUrl: '~/App/tenant/views/OldList/viewAccountsReceivableReport.cshtml'
        });
        $stateProvider.state('tenant.viewCashOnHandReport', {
            url: '/viewCashOnHandReport',
            templateUrl: '~/App/tenant/views/OldList/viewCashOnHandReport.cshtml'
        });
        $stateProvider.state('tenant.viewCashInBankReport', {
            url: '/viewCashInBankReport',
            templateUrl: '~/App/tenant/views/OldList/viewCashInBankReport.cshtml'
        });
        //First Ref
        $stateProvider.state('tenant.ViewFirstRef', {
            url: '/ViewFirstRef',
            templateUrl: '~/App/tenant/views/OldList/ViewFirstRef.cshtml'
        });
        //Revenue
        $stateProvider.state('tenant.ViewRevenue', {
            url: '/ViewRevenue',
            templateUrl: '~/App/tenant/views/OldList/ViewRevenue.cshtml'
        });
        //Upload
        $stateProvider.state('tenant.ViewLibraryUpload', {
            url: '/ViewLibraryUpload',
            templateUrl: '~/App/tenant/views/OldList/ViewLibraryUpload.cshtml'
        });
        //Download
        $stateProvider.state('tenant.ViewLibraryDownload', {
            url: '/ViewLibraryDownload',
            templateUrl: '~/App/tenant/views/OldList/ViewLibraryDownload.cshtml'
        });
        
        //$qProvider settings  
        $qProvider.errorOnUnhandledRejections(false);
    }
]);

appModule.run(["$rootScope", "settings", "$state", 'i18nService', '$uibModalStack', function ($rootScope, settings, $state, i18nService, $uibModalStack) {
    $rootScope.$state = $state;
    $rootScope.$settings = settings;

    $rootScope.$on('$stateChangeSuccess', function () {
        $uibModalStack.dismissAll();
    });

    //Set Ui-Grid language
    if (i18nService.get(abp.localization.currentCulture.name)) {
        i18nService.setCurrentLang(abp.localization.currentCulture.name);
    } else {
        i18nService.setCurrentLang("en");
    }

    $rootScope.safeApply = function (fn) {
        var phase = this.$root.$$phase;
        if (phase == '$apply' || phase == '$digest') {
            if (fn && (typeof (fn) === 'function')) {
                fn();
            }
        } else {
            this.$apply(fn);
        }
    };
}]);

//Written by David Votrubec from ST-Software.com
//Inspired by http://jsfiddle.net/KPeBD/2/
//Up-to-date code can be found on GitHub https://github.com/ST-Software/STAngular/blob/master/src/directives/SgNumberInput
appModule.directive('sgNumberInput', ['$filter', '$locale', function ($filter, $locale) {
    return {
        require: 'ngModel',
        restrict: "A",
        link: function ($scope, element, attrs, ctrl) {
            var fractionSize = parseInt(attrs['fractionSize']) || 0;
            var numberFilter = $filter('number');
            //format the view value
            ctrl.$formatters.push(function (modelValue) {
                var retVal = numberFilter(modelValue, fractionSize);
                var isValid = isNaN(modelValue) == false;
                ctrl.$setValidity(attrs.name, isValid);
                return retVal;
            });
            //parse user's input
            ctrl.$parsers.push(function (viewValue) {
                var caretPosition = getCaretPosition(element[0]), nonNumericCount = countNonNumericChars(viewValue);
                viewValue = viewValue || '';
                //Replace all possible group separators
                var trimmedValue = viewValue.trim().replace(/,/g, '').replace(/`/g, '').replace(/'/g, '').replace(/\u00a0/g, '').replace(/ /g, '');
                //If numericValue contains more decimal places than is allowed by fractionSize, then numberFilter would round the value up
                //Thus 123.109 would become 123.11
                //We do not want that, therefore I strip the extra decimal numbers
                var separator = $locale.NUMBER_FORMATS.DECIMAL_SEP;
                var arr = trimmedValue.split(separator);
                var decimalPlaces = arr[1];
                if (decimalPlaces != null && decimalPlaces.length > fractionSize) {
                    //Trim extra decimal places
                    decimalPlaces = decimalPlaces.substring(0, fractionSize);
                    trimmedValue = arr[0] + separator + decimalPlaces;
                }
                var numericValue = parseFloat(trimmedValue);
                var isEmpty = numericValue == null || viewValue.trim() === "";
                var isRequired = attrs.required || false;
                var isValid = true;
                if (isEmpty && isRequired) {
                    isValid = false;
                }
                if (isEmpty == false && isNaN(numericValue)) {
                    isValid = false;
                }
                ctrl.$setValidity(attrs.name, isValid);
                if (isNaN(numericValue) == false && isValid) {
                    var newViewValue = numberFilter(numericValue, fractionSize);
                    element.val(newViewValue);
                    var newNonNumbericCount = countNonNumericChars(newViewValue);
                    var diff = newNonNumbericCount - nonNumericCount;
                    var newCaretPosition = caretPosition + diff;
                    if (nonNumericCount == 0 && newCaretPosition > 0) {
                        //newCaretPosition--;
                    }
                    setCaretPosition(element[0], newCaretPosition);
                }
                return isNaN(numericValue) == false ? numericValue : null;
            });
        } //end of link function
    };
    //#region helper methods
    function getCaretPosition(inputField) {
        // Initialize
        var position = 0;
        // IE Support
        if (document.selection) {
            inputField.focus();
            // To get cursor position, get empty selection range
            var emptySelection = document.selection.createRange();
            // Move selection start to 0 position
            emptySelection.moveStart('character', -inputField.value.length);
            // The caret position is selection length
            position = emptySelection.text.length;
        }
        else if (inputField.selectionStart || inputField.selectionStart == 0) {
            position = inputField.selectionStart;
        }
        return position;
    }
    function setCaretPosition(inputElement, position) {
        if (inputElement.createTextRange) {
            var range = inputElement.createTextRange();
            range.move('character', position);
            range.select();
        }
        else {
            if (inputElement.selectionStart) {
                inputElement.focus();
                inputElement.setSelectionRange(position, position);
            }
            else {
                inputElement.focus();
            }
        }
    }
    function countNonNumericChars(value) {
        return (value.match(/[^a-z0-9]/gi) || []).length;
    }
    //#endregion helper methods
}]);