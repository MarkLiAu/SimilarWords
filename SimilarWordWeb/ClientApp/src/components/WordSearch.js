import React, { Component, Fragment } from 'react';
import { Link } from 'react-router-dom';
import { Col, Grid, Row,Checkbox, Panel, PanelGroup } from 'react-bootstrap';
import DisplayWord from './DisplayWord';
import { DashBoard } from './DashBoard';

export class WordSearch extends Component {
    displayName = WordSearch.name;

    constructor(props) {
        super(props);
        console.log("constructor");
        this.state = { word2search: '', wordInput: '', frequency: 10000 };
        let w = typeof (this.props.match === null || this.props.match.params.name) === 'undefined' ? '' : this.props.match.params.name;
        console.log(w);
        this.state = { word2search: w, wordInput: w, frequency: 10000, activeKey: '0' };
        this.fetchData();
        //this.handleSelect = this.handleSelect.bind(this);
    }

    componentDidMount() {
        console.log("WordSearch componentDidMount start");
        //this.fetchData();
    }

    componentDidUpdate(prevProps) {
        console.log("WordSearch componentDidUpdate start");
        if (prevProps.match.params.name !== this.props.match.params.name) {
            this.fetchData();
        }
    }

    fetchData() {
        console.log("WordSearch fetchData start");
        this.SearchWord(this.state.word2search);
        if (this.state.word2search.length > 0) document.title = this.state.word2search + '-Similar Word';
    }



    WordChanged = (e) => {
        this.setState({
            wordInput: e.target.value
        });
    }

    SearchWord = (word) => {
        console.log("SearchWord:" + word);
        console.log(this.state);
        if (word.length <= 0) return;
        //if (word === this.state.word2search) return;
        //return;
        console.log('WordSearch Start fetch:');
        fetch('api/Words/' + word)
            .then(response => response.json())
            .then(data => {
                console.log('fetch back');
                // this.props.history.push('/Wordsearch/' + word);
                this.setState({ words: data, word2search: word, wordInput: '', activeKey: '0'  });
            });
    }

    SubmitSearch = () => {
        console.log('SubmitSearch');
        this.SearchWord(this.state.wordInput);
    };

    KeyPressed = (event) => {
        if (event.key === 'Enter') {
            this.SubmitSearch();
        }
    }

    handleSelect(activeKey) {
        console.log('in handleSelect: activeKey=' + activeKey);
        this.setState({ activeKey });
    }

    render() {
        console.log('render');
        console.log("start in WordSearch");
        console.log(this.props);


        return (
            <div className='bj_center'>
                <Link to={'/Wordmemory'} > Start Memory </Link>
                <DashBoard></DashBoard>
                <Panel>
                <input value={this.state.wordInput} placeholder={this.state.word2search} onChange={this.WordChanged} onKeyPress={this.KeyPressed} ></input>
                <span>{' '}</span>
                <button type="submit" onClick={this.SubmitSearch}>Search</button>
                </Panel>
                <PanelGroup accordion id="accordion-example" defaultActiveKey='0' activeKey={this.state.activeKey} onSelect={(x) => this.handleSelect(x)}  >

                    <ShowListComp maxFeq={this.state.frequency} list={this.state.words}></ShowListComp>
                </PanelGroup>
            </div>
        );
    }
}
        
        
const ShowListComp = ({maxFeq, list }) => {
    console.log('ShowList in WordSearch');
    if (typeof (list) === "undefined") return (<div></div>);
    console.log(maxFeq);
    return (
            list.map( (w,idx) => {
                return (
                    <DisplayWord key={w.name} word={w} idx={idx} SearchWord={this.SearchWord}></DisplayWord>
                )
            })

    );
}
