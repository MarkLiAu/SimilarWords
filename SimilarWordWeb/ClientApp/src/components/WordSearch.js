import React, { Component, Fragment } from 'react';
import { Link } from 'react-router-dom';
import { Col, Grid, Row,Checkbox, Panel, PanelGroup } from 'react-bootstrap';
import DisplayWord from './DisplayWord';
import { DashBoard } from './DashBoard';

const ShowListComp = ({ maxFeq, list, handleWordClicked }) => {
    console.log('ShowList in WordSearch');
    if (typeof (list) === "undefined") return (<div></div>);
    console.log(maxFeq);
    return (
        list.map((w, idx) => {
            return (
                <DisplayWord key={w.name} word={w} idx={idx} handleWordClicked={handleWordClicked}></DisplayWord>
            )
        })

    );
}

export class WordSearch extends Component {
    displayName = WordSearch.name;

    constructor(props) {
        super(props);
        console.log("WordSearch constructor");
        //this.state = { word2search: '', wordInput: '', frequency: 10000 };
        console.log(this.props);
        let w = '';
        if (this.props.match && this.props.match.params && this.props.match.params.name)  w = this.props.match.params.name;
//        let w = (this.props.match === undefined || this.props.match.params == undefined || this.props.match.params.name) === 'undefined' ? '' : this.props.match.params.name;
        if (typeof (w) === 'undefined') w = '';
        console.log("w="+w);
        this.state = { word2search: w, wordInput: w, frequency: 10000, activeKey: '0' };
        this.SearchWord(this.state.word2search,false);
        //this.handleSelect = this.handleSelect.bind(this);
    }

    componentDidMount() {
        console.log("WordSearch componentDidMount start");
        // this.SearchWord(this.state.word2search);
    }

    componentDidUpdate(prevProps) {
        console.log("wordsearch componentDidUpdate");
        console.log(this.props);
        console.log(prevProps);
        let w = '';
        if (this.props.match && this.props.match.params && this.props.match.params.name)
            w = this.props.match.params.name;
        let prevW = '';
        if (prevProps && prevProps.match && prevProps.match.params && prevProps.match.params.name)
            prevW = prevProps.match.params.name;

        console.log('wordsearch didupdate, w=' + w + ', prev=' + prevW);
        if(w.trim()!=='' && w!==prevW) {
            //this.state = { word2search: w, wordInput: w, frequency: 10000, activeKey: '0' };
            this.SearchWord(w,false);
        }
    }

    WordChanged = (e) => {
        this.setState({
            wordInput: e.target.value
        });
    }

    SearchWord = (word,bPushHistory=true) => {
        console.log("SearchWord:" + word);
        //console.log(this.state);
        if (word.length <= 0) word="randomword";
        //if (word === this.state.word2search) return;
        //return;
        console.log('WordSearch Start fetch:');
        fetch('api/Words/' + word)
            .then(response => response.json())
            .then(data => {
                console.log('fetch back');
                let lastWord = word;
                if (data.length > 0 && data[0].name) lastWord = data[0].name;
                this.setState({ words: data, word2search: lastWord, wordInput: '', activeKey: '0'  });
                if (bPushHistory||lastWord!=word) this.props.history.push('/Wordsearch/' + lastWord);
            })
            .catch(err => console.log(`Error with message: ${err}`));
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
        console.log("render start in WordSearch");
        console.log(this.props);


        return (
            <div className='bj_center'>
                <Link to={'/Wordmemory'} ><button className='btn btn-info btn-sm'> Start Memory </button></Link>
                <DashBoard></DashBoard>
                <Panel>
                <input value={this.state.wordInput} placeholder={this.state.word2search.length<=0? 'search here': this.state.word2search} onChange={this.WordChanged} onKeyPress={this.KeyPressed} ></input>
                <span>{' '}</span>
                    <button type="submit" className='btn btn-primary' onClick={this.SubmitSearch}><span className="glyphicon glyphicon-search"></span> </button>
                </Panel>
                <PanelGroup accordion id="accordion-example" defaultActiveKey='0' activeKey={this.state.activeKey} onSelect={(x) => this.handleSelect(x)}  >

                    <ShowListComp maxFeq={this.state.frequency} list={this.state.words} handleWordClicked={this.SearchWord}></ShowListComp>
                </PanelGroup>
            </div>
        );
    }
}
        
        
