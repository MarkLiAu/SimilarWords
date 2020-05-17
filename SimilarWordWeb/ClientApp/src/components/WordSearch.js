import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Col, Grid, Row } from 'react-bootstrap';

const ListHeader = () => <Row className="descriptionDiv">
    <Col xs={3} md={1}>word</Col>
    <Col xs={1} md={1}>Seq</Col>
    <Col xs={3} md={1}>Pronounciation</Col>
    <Col xs={4} md={2}>Dictionary</Col>
</Row>

export class WordSearch extends Component {
  displayName = WordSearch.name

  constructor(props) {
      super(props);
      console.log("constructor");

      let w = typeof (this.props.match === null || this.props.match.params.name) === 'undefined' ? '' : this.props.match.params.name; 
      console.log(w);
      this.state = { word2search: w,  wordInput: w, frequency: 10000  };
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
        console.log("SearchWord:"+word);
        console.log(this.state);
        if (word.length <= 0) return;
        //return;
        fetch('api/Words/' + word)
            .then(response => response.json())
            .then(data => {
                console.log('fetch back');
                this.props.history.push('/Wordsearch/' + word);
                this.setState({ words: data, word2search:word, wordInput:'' });
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

    ShowWordName = (name) => {
        if (name == this.state.word2search) {
            return <b className='text-primary'>{name}</b>
        }
        else {
            return <Link to={'/WordSearch/' + name} onClick={() => { this.SearchWord(name) }} >{name} </Link> 
        }
    }

    ShowList = (list) => {
        console.log('ShowList');
        if (typeof (list) === "undefined") return (<div></div>);
        return (
            <Grid fluid>
                <ListHeader> </ListHeader>
                {list.map(w => {
                    if (w.name.toLowerCase()!=this.state.word2search.toLowerCase().trim() &&  Number(w.frequency) > this.state.frequency) return '';
                    return (
                        <Row key={w.name} className="descriptionDiv" style={{ borderBottom: 1 + 'px' }} >
                            <Col xs={3} md={1}> {this.ShowWordName(w.name) } </Col>
                            <Col xs={1} md={1}>{w.frequency}</Col>
                            <Col xs={3} md={1}>{w.pronounciation}</Col>
                            <Col xs={4} md={2}>
                                <a title="Collins" href={'https://www.collinsdictionary.com/dictionary/english/' + w.name} target="_blank">CL</a>
                                <span>|</span>
                                <a title='Longman' href={'https://www.ldoceonline.com/dictionary/' + w.name} target="_blank">LM</a>
                                <span>|</span>
                                <a title='Merriam Webster' href={'https://www.merriam-webster.com/dictionary/' + w.name} target="_blank">MW</a>
                                <span>|</span>
                                <a title='Oxford Learners' href={'https://www.oxfordlearnersdictionaries.com/definition/english/' + w.name+'_1'} target="_blank">OX</a>
                                <span>|</span>
                                <a title='Cambridge' href={'https://dictionary.cambridge.org/dictionary/english/' + w.name} target="_blank">CA</a>
                                <span>|</span>
                                <a title='Macmilland' href={'https://www.macmillandictionary.com/dictionary/british/' + w.name + '_1'} target="_blank">MA</a>
                                <span>|</span>
                                <a title='Lexico' href={'https://www.lexico.com/en/definition/' + w.name} target="_blank">LX</a>
                            </Col>
                        <Col  xsOffset={1} xs={11} mdOffset={0} md={7}>{w.meaningShort}</Col>
                        </Row>
                    )})
                }

            </Grid>


        );
    }

    ShowAllChanged = (e) => {
        console.log( 'ShowAllChanged');
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
        <div>
            <h3>Similar Words</h3>

                <input value={this.state.wordInput} placeholder={this.state.word2search} onChange={this.WordChanged} onKeyPress={this.KeyPressed} ></input>
            <button type="submit" onClick={this.SubmitSearch}>Search</button>
            <input className="form-check-input" type="checkbox" id="Showall" name="Showall" onChange={this.ShowAllChanged}></input>
            <label className="form-check-label" >Show All</label>

            {this.ShowList(this.state.words)}
        </div>
    );
  }
}
