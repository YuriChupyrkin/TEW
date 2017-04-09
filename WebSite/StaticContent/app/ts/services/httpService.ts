import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ConstantStorage } from '../helpers/constantStorage'
import { PubSub } from './pubSub';

import 'rxjs/add/operator/map';

@Injectable()
export class HttpService {

    constructor(private http: Http) {
    }

    public processGet<T>(url: string, isExternalRequest = false): Promise<T> {
        let headers = isExternalRequest ? null : this.buildAuthHeaders();

        // start loading...
        PubSub.Pub(ConstantStorage.getLoadingEvent(), true);

        let getRequest = this.http.get(url, { headers: headers}).map(response => <T>response.json());

        let promise = new Promise<T>((resolve, reject) => {
            getRequest.subscribe(
                r => {
                    PubSub.Pub(ConstantStorage.getLoadingEvent(), false);
                    resolve(r);
                },
                e => {
                    console.error(`${url} (GET): request finished with error:`);
                    console.error(e);
                    PubSub.Pub(ConstantStorage.getLoadingEvent(), false);
                    reject(e);
                }
            );

        });
        return promise;
    }

    public processPost(object: any, url: string) {
        let headers = this.buildAuthHeaders();

        // start loading...
        PubSub.Pub(ConstantStorage.getLoadingEvent(), true);

        let postRequest = this.http.post(url, object, { headers: headers });

        let promise = new Promise((resolve, reject) => {
            postRequest.subscribe(
                r => {
                    PubSub.Pub(ConstantStorage.getLoadingEvent(), false);
                    resolve(r);
                },
                e => {
                    console.error(`${url} (POST): request finished with error:`);
                    console.error(e);
                    PubSub.Pub(ConstantStorage.getLoadingEvent(), false);
                    reject(e);
                }
            );
        });

        return promise;
    }

    private buildAuthHeaders () {
        let headers = new Headers();

        let userId = ConstantStorage.getUserId();
        let uniqueId = ConstantStorage.getUserUniqueId();
        if (userId && uniqueId) {
            headers.append('Authorization', `${userId.toString()}|${uniqueId}`);
        }

        return headers;
    }
}