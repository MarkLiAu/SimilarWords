import * as React from 'react';
import { RouteComponentProps } from 'react-router';


interface WordSearchState {
    name: string;
    pronounciation: string;
    honomony: string;
    explanationShort: string;
    explanationLong: string
}

export class WordSearch extends React.Component<RouteComponentProps<{}>, WordSearchState> {
    constructor() {
        super();
        this.state = { name: '', pronounciation: '', honomony: '', explanationShort: '', explanationLong:'' };
    }



    public render() {
        return <div>
            <h1>English Word</h1>

            <input onChange={this.WordInput}></input>

            <p>Current count: <strong>{ this.state.name }</strong></p>

            <button onClick={() => { this.search() }}>Search</button>
            <div>{this.state.name} </div>
            <div>{this.state.pronounciation} </div>
            <div>{this.state.honomony} </div>
            <div>{this.state.explanationShort} </div>
            <div><b>{this.state.explanationLong}</b> </div>
       </div>;
    }

    WordInput = (e: any)=> {
        this.setState({
            name: e.target.value
        });
    }

    search() {
        //alert(this.state.name);
        fetch('api/Words/'+this.state.name)
            .then(response => response.json() as Promise<WordSearchState>)
            .then(data => {
                this.setState( data);
            });

    }
}
