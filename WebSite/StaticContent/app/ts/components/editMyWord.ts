import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpService } from '../services/httpService';
import { Word } from '../models/word';
import { ModalWindowServise } from '../services/modalWindowServise';
import { PubSub } from '../services/pubSub';
import { WordsCloudModel } from '../models/wordsCloudModel';
import { ConstantStorage } from '../helpers/constantStorage';
import '../../scss/components/common.scss';
import '../../scss/components/editMyWord.scss';

@Component({
    selector: 'edit-my-word',
    templateUrl: '../../StaticContent/app/templates/components/editMyWord.html',
})

export class EditMyWord {
    private editWordform: FormGroup;
    private word;

    @Input() set options(options: any) {
        this.editWordform = this.formBuilder.group({
            Example: [options.Example, Validators.maxLength(50)],
            Russian: [options.Russian, Validators.compose([Validators.required, Validators.maxLength(25)])],
        });

        this.word = options;
    }

    constructor(private formBuilder: FormBuilder, private httpService: HttpService) {
    }

    private cancel(): void {
        PubSub.Pub('modalClose');
    }

    private save(value: any): void {
        let updateRussian = value['Russian'];
        let updateExample = value['Example'];

        if (updateRussian === this.word.Russian &&
            updateExample === this.word.Example) {
            PubSub.Pub('modalClose');
            return;
        }

        let updateWord = new Word();
        updateWord.English = this.word.English;
        updateWord.Id = this.word.Id;
        updateWord.Russian = updateRussian;
        updateWord.Example = updateExample;

        // reset level if translate has been updated
        updateWord.FailAnswerCount = updateRussian === this.word.Russian ? this.word.FailAnswerCount : 0;
        updateWord.AnswerCount = updateRussian === this.word.Russian ? this.word.AnswerCount : 0;
        updateWord.Level = updateRussian === this.word.Russian ? this.word.Level : 0;

        let wordsCloudModel = new WordsCloudModel();
        wordsCloudModel.Words = [updateWord];
        wordsCloudModel.UserId = ConstantStorage.getUserId();

        this.httpService.processPost(wordsCloudModel, ConstantStorage.getEditWordController());

        PubSub.Pub('editWord', updateWord);
        PubSub.Pub('modalClose');
    }
}
