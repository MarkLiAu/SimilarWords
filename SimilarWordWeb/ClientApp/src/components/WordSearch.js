import React, { Component, Fragment } from 'react';
import { Link } from 'react-router-dom';
import { Col, Grid, Row,Checkbox, Panel } from 'react-bootstrap';
import DisplayWord from './DisplayWord';

const ListHeader = () => <Row className="descriptionDiv">
    <Col xs={3} md={1}>word</Col>
    <Col xs={1} md={1}>Seq</Col>
    <Col xs={3} md={1}>Pronounciation</Col>
    <Col xs={4} md={2}>Dictionary</Col>
</Row>

const ShowDictLink = (name) => {
    return (
        <Fragment>
            <a title="Collins" href={'https://www.collinsdictionary.com/dictionary/english/' + name} target="_blank">CL</a>
            <span>|</span>
            <a title='Longman' href={'https://www.ldoceonline.com/dictionary/' + name} target="_blank">LM</a>
            <span>|</span>
            <a title='Merriam Webster' href={'https://www.merriam-webster.com/dictionary/' + name} target="_blank">MW</a>
            <span>|</span>
            <a title='Oxford Learners' href={'https://www.oxfordlearnersdictionaries.com/definition/english/' + name + '_1'} target="_blank">OX</a>
            <span>|</span>
            <a title='Cambridge' href={'https://dictionary.cambridge.org/dictionary/english/' + name} target="_blank">CA</a>
            <span>|</span>
            <a title='Macmilland' href={'https://www.macmillandictionary.com/dictionary/british/' + name + '_1'} target="_blank">MA</a>
            <span>|</span>
            <a title='Lexico' href={'https://www.lexico.com/definition/' + name} target="_blank">LX</a>
        </Fragment>
    )
}

export class WordSearch extends Component {
    displayName = WordSearch.name

    constructor(props) {
        super(props);
        console.log("constructor");

        let w = typeof (this.props.match === null || this.props.match.params.name) === 'undefined' ? '' : this.props.match.params.name;
        console.log(w);
        this.state = { word2search: w, wordInput: w, frequency: 10000 };
        this.SearchWord(w);
    }
    componentDidMount() {
        console.log('componentDidMount');
        document.title = this.state.word2search + '-Similar Word';
    }

    componentDidUpdate() {
        console.log('componentDidUpdate');
        document.title = this.state.word2search + '-Similar Word';
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
        fetch('api/Words/' + word)
            .then(response => response.json())
            .then(data => {
                console.log('fetch back');
                this.props.history.push('/Wordsearch/' + word);
                this.setState({ words: data, word2search: word, wordInput: '' });
            });
    }

    SubmitSearch = () => {
        console.log('SubmitSearch');
        this.SearchWord(this.state.wordInput);
    };

    //LoadList = () => {
    //    console.log("LoadList");
    //    console.log(this.state);
    //    if (this.state.word2search.length <= 0) return;
    //    console.log("fetch");
    //    fetch('api/Words/' + this.state.word2search)
    //        .then(response => response.json())
    //        .then(data => {
    //            this.setState({ words: data });
    //        });
    //};

    //ShowWordName = (name) => {
    //    if (name == this.state.word2search) {
    //        return <b className='text-primary'>{name}</b>
    //    }
    //    else {
    //        return <Link to={'/WordSearch/' + name} onClick={() => { this.SearchWord(name) }} >{name} </Link>
    //    }
    //}



    ShowAllChanged = (e) => {
        console.log('ShowAllChanged');
        this.setState({
            frequency: e.target.checked ? 50000 : 10000
        });

    }

    KeyPressed = (event) => {
        if (event.key === 'Enter') {
            this.SubmitSearch();
        }
    }
    render() {
        console.log('render');
        return (
            <div className='bj_center'>
                <Panel>
                <input value={this.state.wordInput} placeholder={this.state.word2search} onChange={this.WordChanged} onKeyPress={this.KeyPressed} ></input>
                <span>{' '}</span>
                <button type="submit" onClick={this.SubmitSearch}>Search</button>
                <span>{' '}</span>
                <Checkbox inline onChange={this.ShowAllChanged} > Show All </Checkbox>
                 </Panel>
                <ShowListComp maxFeq={this.state.frequency}  list={this.state.words}></ShowListComp>
            </div>
        );
    }
}
        
        
const ShowListComp = ({maxFeq, list }) => {
    console.log('ShowList');
    if (typeof (list) === "undefined") return (<div></div>);
    console.log(maxFeq);
    return (
            list.map( (w,idx) => {
                if (idx>0 && Number(w.frequency) > maxFeq) return '';
                return (
                    <DisplayWord key={w.name} word={w} idx={idx} SearchWord={this.SearchWord}></DisplayWord>
                )
            })

    );
}
