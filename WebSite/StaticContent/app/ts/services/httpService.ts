import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ConstantStorage } from './constantStorage'
import { PubSub } from './pubSub';

import 'rxjs/add/operator/map';

@Injectable()
export class HttpService {

    constructor(private http: Http) { 
    }

    public processGet<T>(url: string, isExternalRequest = false): Observable<T> {
        let headers = new Headers();
        let userId = ConstantStorage.getUserId()

        if (isExternalRequest == false && userId) {
            headers.append('Authorization', userId.toString());
        }

        // start loading...
        PubSub.Pub(ConstantStorage.getLoadingEvent(), true);

        var getRequest = this.http.get(url, { headers: headers}).map(response => <T>response.json());
        //getRequest.subscribe(r => this.requestFinished(), e => this.requestFinishedWithError(url, 'get', e));
        return getRequest;
    }

    public processPost<T>(object: T, url: string) {
        let headers = new Headers();

        let userId = ConstantStorage.getUserId()
        if (userId) {
            headers.append('Authorization', userId.toString());
        }

        // start loading...
        PubSub.Pub(ConstantStorage.getLoadingEvent(), true);

        var postRequest = this.http.post(url, object, { headers: headers });
        //postRequest.subscribe(r => this.requestFinished(), e => this.requestFinishedWithError(url, 'post', e));
        return postRequest;
    }

    private requestFinished() {
        // end loading... 
        PubSub.Pub(ConstantStorage.getLoadingEvent(), false);
    }

    private requestFinishedWithError(url: string, method: string, error: any) {
        console.log(`${url} (${method}): request finished with error:`);
        console.log(error);

        // end loading...
        PubSub.Pub(ConstantStorage.getLoadingEvent(), false);
    }
}