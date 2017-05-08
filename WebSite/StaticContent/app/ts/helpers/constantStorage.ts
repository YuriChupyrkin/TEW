import { UserStatModel } from '../models/userStatModel';

export class ConstantStorage {
    private static yandexTranslaterApiKey: string;
    private static loadingEvent = 'loading';
    private static userStatModel: UserStatModel;

    // urls
    private static readonly wordTranslaterController = '/api/WordTranslater';
    private static readonly wordsManagerController = '/api/WordsManager';
    private static readonly userInfoController = '/api/UserInfo';
    private static readonly applicationMessageController = '/api/ApplicationMessage';
    private static readonly userStatController = '/api/userStat';
    private static readonly deleteWordController = '/api/DeleteWord';
    private static readonly pickerTestsController = '/api/PickerTests';
    private static readonly wordsLevelUpdaterController = '/api/WordsLevelUpdater';
    private static readonly tewInfoContoller = '/api/TewInfo';
    private static readonly resetWordsLevelContoller = '/api/ResetWordsLevel';
    private static readonly editWordController = '/api/EditWord';
    private static readonly writeTestsController = '/api/WriteTests';

    public static getWriteTestsController() {
        return this.writeTestsController;
    }

    public static getEditWordController() {
        return this.editWordController;
    }

    public static getResetWordsLevelController() {
        return this.resetWordsLevelContoller;
    }

    public static getTewInfoContoller() {
        return this.tewInfoContoller;
    }

    public static getWordsLevelUpdaterController() {
        return this.wordsLevelUpdaterController;
    }

    public static getPickerTestsController() {
        return this.pickerTestsController;
    }

    public static getDeleteWordController() {
        return this.deleteWordController;
    }

    public static getUserStatController() {
        return this.userStatController;
    }

    public static getApplicationMessageController() {
        return this.applicationMessageController;
    }

    public static getUserInfoController() {
        return this.userInfoController;
    }

    public static getWordTranslaterController() {
        return this.wordTranslaterController;
    }

    public static getWordsManagerController() {
        return this.wordsManagerController;
    }

    // end of urls

    public static getLoadingEvent() {
        return this.loadingEvent;
    }

    public static getUserName() {
        return this.userStatModel ? this.userStatModel.Email : '';
    }

    public static setYandexTranslaterApiKey(apiKey: string) {
        this.yandexTranslaterApiKey = apiKey;
    }

    public static getYandexTranslaterApiKey() {
        return this.yandexTranslaterApiKey;
    }

    public static getUserId() {
        return this.userStatModel ? this.userStatModel.Id : 0;
    }

    public static setUserStatModel (userModel: UserStatModel) {
        this.userStatModel = userModel;
    }

    public static getUserStatModel () {
        return this.userStatModel;
    }

    public static getUserUniqueId () {
        return this.userStatModel ? this.userStatModel.UniqueId : '';
    }
}